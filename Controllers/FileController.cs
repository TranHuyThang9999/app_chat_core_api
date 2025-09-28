using Microsoft.AspNetCore.Mvc;
using PaymentCoreServiceApi.Services;
using PaymentCoreServiceApi.Common;

namespace PaymentCoreServiceApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FileController : ControllerBaseCustom
    {
        private readonly IMinIOService _minIOService;
        private readonly ILogger<FileController> _logger;

        public FileController(IMinIOService minIOService, ILogger<FileController> logger)
        {
            _minIOService = minIOService;
            _logger = logger;
        }

        /// <summary>
        /// Upload một file lên MinIO
        /// </summary>
        /// <param name="file">File cần upload</param>
        /// <param name="customFileName">Tên file tùy chỉnh (optional)</param>
        /// <returns>Tên file đã được lưu</returns>
        [HttpPost("upload")]
        public async Task<ActionResult<ApiResponse<FileUploadResponse>>> UploadFile(
            IFormFile file, 
            [FromForm] string? customFileName = null)
        {
            try
            {
                if (file == null || file.Length == 0)
                {
                    return BadRequest(ApiResponse<FileUploadResponse>.BadRequest("Vui lòng chọn file để upload"));
                }

                var fileName = await _minIOService.UploadFileAsync(file, customFileName);
                var fileUrl = await _minIOService.GetFileUrlAsync(fileName);

                var response = new FileUploadResponse
                {
                    FileName = fileName,
                    OriginalFileName = file.FileName,
                    FileSize = file.Length,
                    ContentType = file.ContentType,
                    FileUrl = fileUrl,
                    UploadedAt = DateTime.UtcNow
                };

                _logger.LogInformation($"File '{fileName}' đã được upload thành công bởi user");
                return Ok(ApiResponse<FileUploadResponse>.Success(response, "Upload file thành công"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Lỗi khi upload file: {ex.Message}");
                return BadRequest(ApiResponse<FileUploadResponse>.BadRequest($"Upload file thất bại: {ex.Message}"));
            }
        }

        /// <summary>
        /// Upload nhiều file cùng lúc
        /// </summary>
        /// <param name="files">Danh sách file cần upload</param>
        /// <returns>Danh sách kết quả upload</returns>
        [HttpPost("upload-multiple")]
        public async Task<ActionResult<ApiResponse<List<FileUploadResponse>>>> UploadMultipleFiles(
            List<IFormFile> files)
        {
            try
            {
                if (files == null || files.Count == 0)
                {
                    return BadRequest(ApiResponse<List<FileUploadResponse>>.BadRequest("Vui lòng chọn ít nhất một file để upload"));
                }

                var responses = new List<FileUploadResponse>();
                var errors = new List<string>();

                foreach (var file in files)
                {
                    try
                    {
                        if (file.Length > 0)
                        {
                            var fileName = await _minIOService.UploadFileAsync(file);
                            var fileUrl = await _minIOService.GetFileUrlAsync(fileName);

                            responses.Add(new FileUploadResponse
                            {
                                FileName = fileName,
                                OriginalFileName = file.FileName,
                                FileSize = file.Length,
                                ContentType = file.ContentType,
                                FileUrl = fileUrl,
                                UploadedAt = DateTime.UtcNow
                            });
                        }
                    }
                    catch (Exception ex)
                    {
                        errors.Add($"File '{file.FileName}': {ex.Message}");
                    }
                }

                if (errors.Any())
                {
                    _logger.LogWarning($"Một số file upload thất bại: {string.Join(", ", errors)}");
                }

                var message = errors.Any() 
                    ? $"Upload hoàn thành với {errors.Count} lỗi" 
                    : "Upload tất cả file thành công";

                return Ok(ApiResponse<List<FileUploadResponse>>.Success(responses, message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Lỗi khi upload multiple files: {ex.Message}");
                return BadRequest(ApiResponse<List<FileUploadResponse>>.BadRequest($"Upload files thất bại: {ex.Message}"));
            }
        }

        /// <summary>
        /// Download file từ MinIO
        /// </summary>
        /// <param name="fileName">Tên file cần download</param>
        /// <returns>File stream</returns>
        [HttpGet("download/{fileName}")]
        public async Task<IActionResult> DownloadFile(string fileName)
        {
            try
            {
                var fileStream = await _minIOService.DownloadFileAsync(fileName);
                var contentType = GetContentType(fileName);
                
                _logger.LogInformation($"File '{fileName}' đã được download thành công");
                return File(fileStream, contentType, fileName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Lỗi khi download file '{fileName}': {ex.Message}");
                return NotFound(ApiResponse<object>.NotFound($"File không tồn tại hoặc download thất bại: {ex.Message}"));
            }
        }

        /// <summary>
        /// Lấy URL tạm thời để truy cập file
        /// </summary>
        /// <param name="fileName">Tên file</param>
        /// <param name="expiryInSeconds">Thời gian hết hạn (giây), mặc định 1 giờ</param>
        /// <returns>URL tạm thời</returns>
        [HttpGet("url/{fileName}")]
        public async Task<ActionResult<ApiResponse<FileUrlResponse>>> GetFileUrl(
            string fileName, 
            [FromQuery] int expiryInSeconds = 3600)
        {
            try
            {
                var url = await _minIOService.GetFileUrlAsync(fileName, expiryInSeconds);
                
                var response = new FileUrlResponse
                {
                    FileName = fileName,
                    Url = url,
                    ExpiryInSeconds = expiryInSeconds,
                    ExpiresAt = DateTime.UtcNow.AddSeconds(expiryInSeconds)
                };

                return Ok(ApiResponse<FileUrlResponse>.Success(response, "Tạo URL thành công"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Lỗi khi tạo URL cho file '{fileName}': {ex.Message}");
                return BadRequest(ApiResponse<FileUrlResponse>.BadRequest($"Tạo URL thất bại: {ex.Message}"));
            }
        }

        /// <summary>
        /// Xóa file khỏi MinIO
        /// </summary>
        /// <param name="fileName">Tên file cần xóa</param>
        /// <returns>Kết quả xóa</returns>
        [HttpDelete("{fileName}")]
        public async Task<ActionResult<ApiResponse<object>>> DeleteFile(string fileName)
        {
            try
            {
                var result = await _minIOService.DeleteFileAsync(fileName);
                
                if (result)
                {
                    _logger.LogInformation($"File '{fileName}' đã được xóa thành công");
                    return Ok(ApiResponse<object>.Success(null, "Xóa file thành công"));
                }
                else
                {
                    return BadRequest(ApiResponse<object>.BadRequest("Xóa file thất bại"));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Lỗi khi xóa file '{fileName}': {ex.Message}");
                return BadRequest(ApiResponse<object>.BadRequest($"Xóa file thất bại: {ex.Message}"));
            }
        }

        private string GetContentType(string fileName)
        {
            var extension = Path.GetExtension(fileName).ToLowerInvariant();
            return extension switch
            {
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".gif" => "image/gif",
                ".pdf" => "application/pdf",
                ".doc" => "application/msword",
                ".docx" => "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                ".txt" => "text/plain",
                ".zip" => "application/zip",
                _ => "application/octet-stream"
            };
        }
    }

    // Response models
    public class FileUploadResponse
    {
        public string FileName { get; set; } = string.Empty;
        public string OriginalFileName { get; set; } = string.Empty;
        public long FileSize { get; set; }
        public string ContentType { get; set; } = string.Empty;
        public string FileUrl { get; set; } = string.Empty;
        public DateTime UploadedAt { get; set; }
    }

    public class FileUrlResponse
    {
        public string FileName { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public int ExpiryInSeconds { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}
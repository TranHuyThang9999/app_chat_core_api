using Minio;
using Minio.DataModel.Args;
using System.Security.Cryptography;
using System.Text;

namespace PaymentCoreServiceApi.Services
{
    public interface IMinIOService
    {
        Task<string> UploadFileAsync(IFormFile file, string? customFileName = null);
        Task<Stream> DownloadFileAsync(string fileName);
        Task<bool> DeleteFileAsync(string fileName);
        Task<string> GetFileUrlAsync(string fileName, int expiryInSeconds = 3600);
    }

    public class MinIOService : IMinIOService
    {
        private readonly IMinioClient _minioClient;
        private readonly string _bucketName;
        private readonly ILogger<MinIOService> _logger;

        public MinIOService(IMinioClient minioClient, IConfiguration configuration, ILogger<MinIOService> logger)
        {
            _minioClient = minioClient;
            _bucketName = configuration["MinIO:BucketName"] ?? "uploads";
            _logger = logger;
        }

        public async Task<string> UploadFileAsync(IFormFile file, string? customFileName = null)
        {
            try
            {
                // Kiểm tra bucket có tồn tại không, nếu không thì tạo mới
                var bucketExistsArgs = new BucketExistsArgs().WithBucket(_bucketName);
                bool found = await _minioClient.BucketExistsAsync(bucketExistsArgs);
                if (!found)
                {
                    var makeBucketArgs = new MakeBucketArgs().WithBucket(_bucketName);
                    await _minioClient.MakeBucketAsync(makeBucketArgs);
                    _logger.LogInformation($"Bucket '{_bucketName}' đã được tạo");
                }

                // Tạo tên file unique nếu không có customFileName
                string fileName = customFileName ?? GenerateUniqueFileName(file.FileName);
                
                // Validate file
                ValidateFile(file);

                // Upload file
                using var stream = file.OpenReadStream();
                var putObjectArgs = new PutObjectArgs()
                    .WithBucket(_bucketName)
                    .WithObject(fileName)
                    .WithStreamData(stream)
                    .WithObjectSize(file.Length)
                    .WithContentType(file.ContentType);

                await _minioClient.PutObjectAsync(putObjectArgs);
                
                _logger.LogInformation($"File '{fileName}' đã được upload thành công");
                return fileName;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Lỗi khi upload file: {ex.Message}");
                throw new Exception($"Upload file thất bại: {ex.Message}");
            }
        }

        public async Task<Stream> DownloadFileAsync(string fileName)
        {
            try
            {
                var memoryStream = new MemoryStream();
                var getObjectArgs = new GetObjectArgs()
                    .WithBucket(_bucketName)
                    .WithObject(fileName)
                    .WithCallbackStream(stream => stream.CopyTo(memoryStream));

                await _minioClient.GetObjectAsync(getObjectArgs);
                memoryStream.Position = 0;
                
                _logger.LogInformation($"File '{fileName}' đã được download thành công");
                return memoryStream;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Lỗi khi download file '{fileName}': {ex.Message}");
                throw new Exception($"Download file thất bại: {ex.Message}");
            }
        }

        public async Task<bool> DeleteFileAsync(string fileName)
        {
            try
            {
                var removeObjectArgs = new RemoveObjectArgs()
                    .WithBucket(_bucketName)
                    .WithObject(fileName);

                await _minioClient.RemoveObjectAsync(removeObjectArgs);
                
                _logger.LogInformation($"File '{fileName}' đã được xóa thành công");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Lỗi khi xóa file '{fileName}': {ex.Message}");
                return false;
            }
        }

        public async Task<string> GetFileUrlAsync(string fileName, int expiryInSeconds = 3600)
        {
            try
            {
                var presignedGetObjectArgs = new PresignedGetObjectArgs()
                    .WithBucket(_bucketName)
                    .WithObject(fileName)
                    .WithExpiry(expiryInSeconds);

                string url = await _minioClient.PresignedGetObjectAsync(presignedGetObjectArgs);
                
                _logger.LogInformation($"URL cho file '{fileName}' đã được tạo thành công");
                return url;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Lỗi khi tạo URL cho file '{fileName}': {ex.Message}");
                throw new Exception($"Tạo URL thất bại: {ex.Message}");
            }
        }

        private string GenerateUniqueFileName(string originalFileName)
        {
            var extension = Path.GetExtension(originalFileName);
            var nameWithoutExtension = Path.GetFileNameWithoutExtension(originalFileName);
            var timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            var randomString = GenerateRandomString(8);
            
            return $"{nameWithoutExtension}_{timestamp}_{randomString}{extension}";
        }

        private string GenerateRandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private void ValidateFile(IFormFile file)
        {
            // Kiểm tra kích thước file (max 10MB)
            const long maxFileSize = 10 * 1024 * 1024; // 10MB
            if (file.Length > maxFileSize)
            {
                throw new Exception($"File quá lớn. Kích thước tối đa cho phép là {maxFileSize / (1024 * 1024)}MB");
            }

            // Kiểm tra file có rỗng không
            if (file.Length == 0)
            {
                throw new Exception("File không được để trống");
            }

            // Kiểm tra extension được phép
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".pdf", ".doc", ".docx", ".txt", ".zip" };
            var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
            
            if (!allowedExtensions.Contains(fileExtension))
            {
                throw new Exception($"Định dạng file không được hỗ trợ. Các định dạng được phép: {string.Join(", ", allowedExtensions)}");
            }
        }
    }
}
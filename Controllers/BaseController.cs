using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;

namespace PaymentCoreServiceApi.Controllers;

public class ApiResponseModel
{
    public int Code { get; set; }
    public string Message { get; set; } = string.Empty;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public object? Data { get; set; }
}

public abstract class BaseController : ControllerBase
{
    protected IActionResult ApiResponse(int code, string message, object? data = null)
    {
        var response = new ApiResponseModel
        {
            Code = code,
            Message = message,
            Data = data
        };
        
        return Ok(response);
    }
    
    protected IActionResult SuccessResponse(object? data = null)
    {
        return ApiResponse(200, "Success", data);
    }
    
    protected IActionResult ErrorResponse(string message, int code = 400)
    {
        return ApiResponse(code, message);
    }
}
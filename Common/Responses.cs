using System.Text.Json.Serialization;

namespace PaymentCoreServiceApi.Common;

public class ApiResponse<T>
{
    [JsonIgnore]
    public int HttpStatus { get; set; } = 200;

    public int Code { get; set; } = 0;
    public string Message { get; set; } = "Success";

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public T? Data { get; set; }
    public ApiResponse() { }

    public ApiResponse(T data, string message = "Success", int code = 0, int httpStatus = 200)
    {
        Data = data;
        Message = message;
        Code = code;
        HttpStatus = httpStatus;
    }

    public ApiResponse(string message, int code = 0, int httpStatus = 400)
    {
        Data = default;
        Message = message;
        Code = code;
        HttpStatus = httpStatus;
    }

    public static ApiResponse<T> Success(T data, string message = "Success")
        => new(data, message, 0, 200);

    public static ApiResponse<T> NotFound(string message = "Not Found", int code = 401)
        => new(message, code, 404);

    public static ApiResponse<T> BadRequest(string message = "Bad Request", int code = 400)
        => new(message, code, 400);

    public static ApiResponse<T> Unauthorized(string message = "Unauthorized", int code = 401)
        => new(message, code, 401);

    public static ApiResponse<T> Forbidden(string message = "Forbidden", int code = 403)
        => new(message, code, 403);

    public static ApiResponse<T> Conflict(string message = "Conflict", int code = 409)
        => new(message, code, 409);

    public static ApiResponse<T> InternalServerError(string message = "Internal Server Error", int code = 500)
        => new(message, code, 500);
}
namespace PaymentCoreServiceApi.Features.Auth;

public class LoginRequest
{
    public string UserName { get; set; } = default!;
    public string Password { get; set; } = default!;
}

public class LoginResponse
{
    public string Token { get; set; } = default!;
    public string RefreshToken { get; set; } = default!;
    public DateTime Expiration { get; set; }
}

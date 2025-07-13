using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace PaymentCoreServiceApi.Services;

public interface IExecutionContext
{
    long Id { get; }
    string? UserName { get; }
    string? Email { get; }
    bool IsAuthenticated { get; }
}

public class CurrentUser : IExecutionContext
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUser(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public long Id
    {
        get
        {
            var idValue = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            return long.TryParse(idValue, out var id) ? id : 0;
        }
    }
    public string? UserName => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Name);
    public string? Email => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Email);
    public bool IsAuthenticated => _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;
}

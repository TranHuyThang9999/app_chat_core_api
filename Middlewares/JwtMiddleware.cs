using PaymentCoreServiceApi.Features.Auth;

namespace PaymentCoreServiceApi.Middlewares;

public class JwtMiddleware : IMiddleware
{
    private readonly IJwtService _jwtService;

    public JwtMiddleware(IJwtService jwtService)
    {
        _jwtService = jwtService;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

        if (token != null)
        {
            var principal = _jwtService.ValidateToken(token);
            if (principal != null)
            {
                context.User = principal;
            }
        }

        await next(context);
    }
}
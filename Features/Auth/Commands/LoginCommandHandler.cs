using MediatR;
using Microsoft.EntityFrameworkCore;
using PaymentCoreServiceApi.Core.Interfaces.Repositories.Write;
using PaymentCoreServiceApi.Infrastructure.DbContexts;
using PaymentCoreServiceApi.Services;

namespace PaymentCoreServiceApi.Features.Auth.Commands;

public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponse>
{
    private readonly AppDbContext _context;
    private readonly IJwtService _jwtService;
    private readonly IExecutionContext _currentUser;

    public LoginCommandHandler(
        AppDbContext context, 
        IJwtService jwtService,
        IExecutionContext currentUser)
    {
        _context = context;
        _jwtService = jwtService;
        _currentUser = currentUser;
    }

    public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.UserName == request.UserName, cancellationToken);

        if (user == null || user.Password != request.Password) 
        {
            throw new UnauthorizedAccessException("Invalid username or password");
        }

        var token = _jwtService.GenerateToken(user);
        
        return new LoginResponse
        {
            Token = token,
            RefreshToken = "",
            Expiration = DateTime.Now.AddHours(1)
        };
    }
}

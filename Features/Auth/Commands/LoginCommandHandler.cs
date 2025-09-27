using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PaymentCoreServiceApi.Common;
using PaymentCoreServiceApi.Common.Mediator;
using PaymentCoreServiceApi.Infrastructure.DbContexts;
using PaymentCoreServiceApi.Services;

namespace PaymentCoreServiceApi.Features.Auth.Commands;

public class LoginCommandHandler : IRequestApiResponseHandler<LoginCommand, LoginResponse>
{
    private readonly AppDbContext _context;
    private readonly IJwtService _jwtService;
    private readonly IExecutionContext _currentUser;
    private readonly IPinHasher _pinHasher;

    public LoginCommandHandler(
        AppDbContext context,
        IJwtService jwtService,
        IExecutionContext currentUser,
        IPinHasher pinHasher)
    {
        _context = context;
        _jwtService = jwtService;
        _currentUser = currentUser;
        _pinHasher = pinHasher;
    }

    public async Task<ApiResponse<LoginResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.UserName) || string.IsNullOrWhiteSpace(request.Password))
        {
            return ApiResponse<LoginResponse>.BadRequest("Username and password are required");
        }

        var user = await _context.Users
            .FirstOrDefaultAsync(u => 
                (u.UserName == request.UserName || u.Email == request.UserName) 
                && u.Active && !u.Deleted, 
                cancellationToken);

        if (user == null)
        {
            return ApiResponse<LoginResponse>.Unauthorized("Invalid username or password");
        }
        if (!_pinHasher.VerifyPin(request.Password, user.Password))
        {
            return ApiResponse<LoginResponse>.Unauthorized("Invalid username or password");
        }

        // Generate JWT token
        var token = _jwtService.GenerateToken(user);
        
        // Create response
        var loginResponse = new LoginResponse
        {
            Token = token,
            RefreshToken = "",
            Expiration = DateTime.Now.AddHours(1),
        };

        return ApiResponse<LoginResponse>.Success(loginResponse);
    }
}
using MediatR;
using Microsoft.EntityFrameworkCore;
using PaymentCoreServiceApi.Infrastructure.DbContexts;
using PaymentCoreServiceApi.Services;

namespace PaymentCoreServiceApi.Features.Users.Queries;

public class GetUserProfileQuery : IRequest<UserProfileResponse>
{
    // Empty because we'll get the user ID from the current context
}

public class UserProfileResponse
{
    public string Id { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateTime LastLogin { get; set; }
    public int ProjectsCount { get; set; }
}

public class GetUserProfileQueryHandler : IRequestHandler<GetUserProfileQuery, UserProfileResponse>
{
    private readonly AppDbContext _context;
    private readonly ICurrentUser _currentUser;

    public GetUserProfileQueryHandler(
        AppDbContext context,
        ICurrentUser currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<UserProfileResponse> Handle(GetUserProfileQuery request, CancellationToken cancellationToken)
    {
        if (!_currentUser.IsAuthenticated)
        {
            throw new UnauthorizedAccessException("User is not authenticated");
        }

        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Id.ToString() == _currentUser.Id, cancellationToken);

        if (user == null)
        {
            throw new KeyNotFoundException($"User with ID {_currentUser.Id} not found");
        }

        return new UserProfileResponse
        {
            Id = user.Id.ToString(),
            UserName = user.UserName,
            Email = user.Email,
            LastLogin = DateTime.UtcNow, // You might want to store this in the database
            ProjectsCount = 0 // You can add a Projects table and count them here
        };
    }
}

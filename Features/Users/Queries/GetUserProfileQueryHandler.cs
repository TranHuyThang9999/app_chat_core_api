using MediatR;
using Microsoft.EntityFrameworkCore;
using PaymentCoreServiceApi.Core.Entities.UserGenerated;
using PaymentCoreServiceApi.Infrastructure.DbContexts;
using PaymentCoreServiceApi.Services;

namespace PaymentCoreServiceApi.Features.Users.Queries;

public class GetUserProfileQueryHandler : IRequestHandler<GetUserProfileQuery, User>
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

    public async Task<User> Handle(GetUserProfileQuery request, CancellationToken cancellationToken)
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


        return user;
    }
}
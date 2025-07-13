using MediatR;
using Microsoft.EntityFrameworkCore;
using PaymentCoreServiceApi.Common;
using PaymentCoreServiceApi.Common.Mediator;
using PaymentCoreServiceApi.Core.Entities.UserGenerated;
using PaymentCoreServiceApi.Infrastructure.DbContexts;
using PaymentCoreServiceApi.Services;

namespace PaymentCoreServiceApi.Features.Users.Queries;
public class GetUserProfileQueryHandler : IRequestApiResponseHandler<GetUserProfileQuery, User>
{
    private readonly AppDbContext _context;
    private readonly IExecutionContext _currentUser;

    public GetUserProfileQueryHandler(
        AppDbContext context,
        IExecutionContext currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<ApiResponse<User>> Handle(GetUserProfileQuery request, CancellationToken cancellationToken)
    {
        if (!_currentUser.IsAuthenticated)
        {
            throw new UnauthorizedAccessException("User is not authenticated");
        }

        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Id == _currentUser.Id, cancellationToken);

        if (user == null)
        {
            throw new KeyNotFoundException($"User with ID {_currentUser.Id} not found");
        }


        return ApiResponse<User>.Success(user);
    }
}
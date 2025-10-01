using PaymentCoreServiceApi.Common;
using PaymentCoreServiceApi.Common.Mediator;
using PaymentCoreServiceApi.Core.Entities.UserGenerated;
using PaymentCoreServiceApi.Core.Interfaces.Repositories.Read;
using PaymentCoreServiceApi.Services;

namespace PaymentCoreServiceApi.Features.Users.Queries;

public class GetUsersQueryHandler : IRequestApiResponseHandler<GetUsersQuery, PagedResult<User>>
{
    private readonly IUserReadRepository _userReadRepository;
    private readonly IExecutionContext _currentUser;

    public GetUsersQueryHandler(
        IUserReadRepository userReadRepository,
        IExecutionContext currentUser)
    {
        _userReadRepository = userReadRepository;
        _currentUser = currentUser;
    }

    public async Task<ApiResponse<PagedResult<User>>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        if (!_currentUser.IsAuthenticated)
        {
            throw new UnauthorizedAccessException("User is not authenticated");
        }

        var result = await _userReadRepository.GetUsersAsync(
            request.Page, 
            request.PageSize, 
            request.SearchTerm, 
            cancellationToken);

        return ApiResponse<PagedResult<User>>.Success(result);
    }
}
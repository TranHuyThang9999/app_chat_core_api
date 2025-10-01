using PaymentCoreServiceApi.Common;
using PaymentCoreServiceApi.Common.Mediator;
using PaymentCoreServiceApi.Core.Entities.UserGenerated;
using PaymentCoreServiceApi.Core.Interfaces.Repositories.Read;
using PaymentCoreServiceApi.Features.Users.DTOs;
using PaymentCoreServiceApi.Services;

namespace PaymentCoreServiceApi.Features.Users.Queries;

public class GetUsersQueryHandler : IRequestApiResponseHandler<GetUsersQuery, PagedResult<UserBasicInfoDto>>
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

    public async Task<ApiResponse<PagedResult<UserBasicInfoDto>>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
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
        var listUser = result.Items.Select(x => new UserBasicInfoDto
        {
            Id = x.Id,
            NickName = x.NickName,
            Avatar = x.Avatar,
            Gender = x.Gender,
            BirthDate = x.BirthDate,
            Age = x.Age,
            Email = x.Email,
            PhoneNumber = x.PhoneNumber,
            Address = x.Address,
        }).ToList();
        
        return ApiResponse<PagedResult<UserBasicInfoDto>>.Success(new PagedResult<UserBasicInfoDto>(listUser, result.TotalCount, request.Page, request.PageSize));
    }
}
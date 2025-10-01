using PaymentCoreServiceApi.Common;
using PaymentCoreServiceApi.Common.Mediator;
using PaymentCoreServiceApi.Features.Users.DTOs;

namespace PaymentCoreServiceApi.Features.Users.Queries;

public class GetUsersQuery : IRequestApiResponse<PagedResult<UserBasicInfoDto>>
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? SearchTerm { get; set; }
}
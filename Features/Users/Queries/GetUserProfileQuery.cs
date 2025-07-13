using MediatR;
using PaymentCoreServiceApi.Common.Mediator;
using PaymentCoreServiceApi.Core.Entities.UserGenerated;

namespace PaymentCoreServiceApi.Features.Users.Queries;

public class GetUserProfileQuery : IRequestApiResponse<User>
{
}
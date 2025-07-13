using MediatR;
using PaymentCoreServiceApi.Core.Entities.UserGenerated;

namespace PaymentCoreServiceApi.Features.Users.Queries;

public class GetUserProfileQuery : IRequest<User>
{
}
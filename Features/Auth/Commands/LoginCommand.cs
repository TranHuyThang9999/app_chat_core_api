using MediatR;
using PaymentCoreServiceApi.Common.Mediator;

namespace PaymentCoreServiceApi.Features.Auth.Commands;

public record LoginCommand : IRequestApiResponse<LoginResponse>
{
    public string UserName { get; init; } = default!;
    public string Password { get; init; } = default!;
}

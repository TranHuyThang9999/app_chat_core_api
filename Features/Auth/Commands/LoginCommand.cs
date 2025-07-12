using MediatR;

namespace PaymentCoreServiceApi.Features.Auth.Commands;

public record LoginCommand : IRequest<LoginResponse>
{
    public string UserName { get; init; } = default!;
    public string Password { get; init; } = default!;
}

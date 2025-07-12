using MediatR;
using PaymentCoreServiceApi.Core.Entities.UserAgents;

namespace PaymentCoreServiceApi.Features.Users.Commands;

public record CreateUserCommand : IRequest<User>
{
    public string Username { get; init; } = default!;
    public string Email { get; init; } = default!;
    public string Password { get; init; } = default!;
}

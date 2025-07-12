using MediatR;
using PaymentCoreServiceApi.Core.Entities.UserAgents;
using PaymentCoreServiceApi.Core.Interfaces.Repositories.Write;

namespace PaymentCoreServiceApi.Features.Users.Commands;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, User>
{
    private readonly IUserWriteRepository _userWriteRepository;

    public CreateUserCommandHandler(IUserWriteRepository userWriteRepository)
    {
        _userWriteRepository = userWriteRepository;
    }

    public async Task<User> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var user = new User
        {
            UserName = request.Username,
            Email = request.Email,
            Password = request.Password // Note: In a real application, you should hash the password
        };

        var result = await _userWriteRepository.AddAsync(user);
        return result;
    }
}

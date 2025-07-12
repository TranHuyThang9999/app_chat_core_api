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
            NickName = request.NickName,
            Avatar = request.Avatar,
            Gender = request.Gender,
            BirthDate = request.BirthDate,
            Age = request.Age,
            Email = request.Email,
            UserName = request.UserName,
            Password = request.Password,
            PhoneNumber = request.PhoneNumber,
            Address = request.Address,
            Active = true,
        };


        var result = await _userWriteRepository.AddAsync(user);
        
        return result;
    }

    public IUserWriteRepository UserWriteRepository => _userWriteRepository;
}

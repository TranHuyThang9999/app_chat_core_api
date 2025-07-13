using MediatR;
using PaymentCoreServiceApi.Core.Entities.UserGenerated;
using PaymentCoreServiceApi.Core.Interfaces.Repositories.Read;
using PaymentCoreServiceApi.Core.Interfaces.Repositories.Write;

namespace PaymentCoreServiceApi.Features.Users.Commands;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, User>
{
    private readonly IUserWriteRepository _userWriteRepository;
    private readonly IUserReadRepository _userReadRepository;
    public CreateUserCommandHandler(IUserWriteRepository userWriteRepository, IUserReadRepository userReadRepository)
    {
        _userWriteRepository = userWriteRepository;
        _userReadRepository = userReadRepository;
    }

    public async Task<User> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        if (await _userReadRepository.ExistsAsync(request.UserName))
        {
            return null;
        }
        try
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
                Active = true
            };

            var result = await _userWriteRepository.AddAsync(user);
            await _userWriteRepository.CommitAsync();
            return result;
        }
        catch (Exception ex)
        {
            throw;
        }
        
    }
}

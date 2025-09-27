using PaymentCoreServiceApi.Common;
using PaymentCoreServiceApi.Common.Mediator;
using PaymentCoreServiceApi.Core.Entities.UserGenerated;
using PaymentCoreServiceApi.Core.Interfaces.Repositories.Read;
using PaymentCoreServiceApi.Core.Interfaces.Repositories.Write;
using PaymentCoreServiceApi.Services;

namespace PaymentCoreServiceApi.Features.Users.Commands;
public class CreateUserCommandHandler: IRequestApiResponseHandler<CreateUserCommand, User>
{
    private readonly IUserWriteRepository _userWriteRepository;
    private readonly IUserReadRepository _userReadRepository;
    private readonly IPinHasher _pinHasher;
    
    public CreateUserCommandHandler(IUserWriteRepository userWriteRepository, IUserReadRepository userReadRepository, IPinHasher pinHasher)
    {
        _userWriteRepository = userWriteRepository;
        _userReadRepository = userReadRepository;
        _pinHasher = pinHasher;
    }

    public async Task<ApiResponse<User>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        if (await _userReadRepository.ExistsAsync(request.UserName))
        {
            return ApiResponse<User>.Conflict();
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
                Password = _pinHasher.HashPin(request.Password),
                PhoneNumber = request.PhoneNumber,
                Address = request.Address,
                Active = true
            };

            var result = await _userWriteRepository.AddAsync(user);
            await _userWriteRepository.CommitAsync();
            
            return ApiResponse<User>.Success(user, "User created successfully");
        }
        catch (Exception ex)
        {
            return ApiResponse<User>.InternalServerError(ex.Message);
        }
        
    }
}
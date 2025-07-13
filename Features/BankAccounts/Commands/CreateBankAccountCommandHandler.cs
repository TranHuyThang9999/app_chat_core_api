using MediatR;
using PaymentCoreServiceApi.Core.Entities.BankAccountGenerated;
using PaymentCoreServiceApi.Core.Interfaces.Repositories.Read;
using PaymentCoreServiceApi.Core.Interfaces.Repositories.Write;
using AutoMapper;
using PaymentCoreServiceApi.Services;

namespace PaymentCoreServiceApi.Features.BankAccounts.Commands;

public class CreateBankAccountCommandHandler: IRequestHandler<CreateBankAccountCommand, BankAccount>
{
    private readonly IUserReadRepository _userReadRepository;
    private readonly IBankAccountWriteRepository _bankAccountWriteRepository;
    private readonly IBankAccountReadRepository _bankAccountReadRepository;
    private readonly IExecutionContext _executionContext;
    private readonly IMapper _mapper;
    private readonly IPinHasher _pinHasher;
    private readonly ILogger<CreateBankAccountCommandHandler> _logger;

    public CreateBankAccountCommandHandler(IUserReadRepository userReadRepository, IBankAccountWriteRepository bankAccountWriteRepository, IBankAccountReadRepository bankAccountReadRepository, IExecutionContext executionContext, IMapper mapper, IPinHasher pinHasher, ILogger<CreateBankAccountCommandHandler> logger)
    {
        _userReadRepository = userReadRepository;
        _bankAccountWriteRepository = bankAccountWriteRepository;
        _bankAccountReadRepository = bankAccountReadRepository;
        _executionContext = executionContext;
        _mapper = mapper;
        _pinHasher = pinHasher;
        _logger = logger;
    }

    public async Task<BankAccount> Handle(CreateBankAccountCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var user = await _userReadRepository.GetByIdAsync(_executionContext.Id);

            if (user == null)
            {
                throw new Exception("User not found");
            }

            if (await _bankAccountReadRepository.ExistsBankAccountByUserIdAsync(_executionContext.Id))
            {
               throw new Exception("User already has a bank account"); 
            }
            var entity = new BankAccount();
            entity.UserId = user.Id;
            entity.AccountNumber = request.IsUsePhone ? user.PhoneNumber : request.AccountNumber;
            entity.Balance = 100000;
            entity.Currency = "VND";
            entity.CodePinHash = _pinHasher.HashPin(request.CodePinHash);
            entity.IsActive = true;

            await _bankAccountWriteRepository.AddAsync(entity);
            await _bankAccountWriteRepository.CommitAsync();

            return entity;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error creating bank account");
            throw;
        }
       
    }

}

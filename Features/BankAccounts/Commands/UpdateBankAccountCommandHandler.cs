using AutoMapper;
using Microsoft.AspNetCore.Http.Extensions;
using PaymentCoreServiceApi.Common;
using PaymentCoreServiceApi.Common.Mediator;
using PaymentCoreServiceApi.Core.Interfaces.Repositories.Read;
using PaymentCoreServiceApi.Core.Interfaces.Repositories.Write;
using PaymentCoreServiceApi.Services;

namespace PaymentCoreServiceApi.Features.BankAccounts.Commands;

public class UpdateBankAccountCommandHandler: IRequestApiResponseHandler<UpdateBankAccountCommand, bool>
{
    private readonly IExecutionContext _executionContext;
    private readonly IBankAccountWriteRepository _bankAccountWriteRepository;
    private readonly IBankAccountReadRepository _bankAccountReadRepository;
    private readonly IMapper _mapper;

    public UpdateBankAccountCommandHandler(IExecutionContext executionContext, IBankAccountWriteRepository bankAccountWriteRepository, IBankAccountReadRepository bankAccountReadRepository, IMapper mapper)
    {
        _executionContext = executionContext;
        _bankAccountWriteRepository = bankAccountWriteRepository;
        _bankAccountReadRepository = bankAccountReadRepository;
        _mapper = mapper;
    }

    public async Task<ApiResponse<bool>> Handle(UpdateBankAccountCommand request, CancellationToken cancellationToken)
    {
        var bankAccount = await _bankAccountReadRepository.GetByIdAsync(request.UpdateBankAccount.Id);
        if (bankAccount == null)
        {
            return ApiResponse<bool>.NotFound($"Bank account with id {request.UpdateBankAccount.Id} not found");
        }
        await _bankAccountWriteRepository.UpdateAsync(request.UpdateBankAccount);
        
        await _bankAccountWriteRepository.CommitAsync();
        
        return ApiResponse<bool>.Success(true);
    }
}
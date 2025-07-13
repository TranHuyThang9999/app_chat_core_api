
using PaymentCoreServiceApi.Common.Mediator;
using PaymentCoreServiceApi.Core.Entities.BankAccountGenerated;

namespace PaymentCoreServiceApi.Features.BankAccounts.Commands;

public class UpdateBankAccountCommand:  IRequestApiResponse<bool>
{ public BankAccount UpdateBankAccount { get; set; }
}
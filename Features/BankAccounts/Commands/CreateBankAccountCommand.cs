using System.ComponentModel.DataAnnotations;
using MediatR;
using PaymentCoreServiceApi.Core.Entities.BankAccountGenerated;

namespace PaymentCoreServiceApi.Features.BankAccounts.Commands;
public class CreateBankAccountCommand : IRequest<BankAccount>
{
    [Required]
    public string? AccountNumber { get; set; }
    [Required]
    [StringLength(6, MinimumLength = 6)]
    public string CodePinHash { get; set; }
    public bool IsUsePhone { get; set; } = false;
}

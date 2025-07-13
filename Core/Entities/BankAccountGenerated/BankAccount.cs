using PaymentCoreServiceApi.Core.Entities.BaseModel;

namespace PaymentCoreServiceApi.Core.Entities.BankAccountGenerated;

public class BankAccount: EntityBase
{
    public string? AccountNumber { get; set; }
    public string? Currency { get; set; }
    public decimal Balance { get; set; }
    public long UserId { get; set; }
    public bool IsActive { get; set; } = true;
    public string CodePinHash { get; set; }
}
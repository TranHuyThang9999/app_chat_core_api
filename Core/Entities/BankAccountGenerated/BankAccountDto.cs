namespace PaymentCoreServiceApi.Core.Entities.BankAccountGenerated;

public class BankAccountDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string AccountNumber { get; set; } = string.Empty;
    public decimal Balance { get; set; }
    public string Currency { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public string? CreatedAt { get; set; }
    public string? UpdatedAt { get; set; }
}

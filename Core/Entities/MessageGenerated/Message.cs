using PaymentCoreServiceApi.Core.Entities.BaseModel;

public class Message: EntityBase
{
    public long SenderId { get; set; }
    public long ReceiverId { get; set; }
    public string Content { get; set; } = string.Empty;
}
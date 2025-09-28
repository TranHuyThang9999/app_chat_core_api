using PaymentCoreServiceApi.Core.Entities.BaseModel;
using PaymentCoreServiceApi.Core.Entities.UserGenerated;

namespace PaymentCoreServiceApi.Core.Entities.MessageGenerated;

public class Message : EntityBase
{
    public long SenderId { get; set; }
    public long ReceiverId { get; set; }
    public string Content { get; set; } = string.Empty;
    public DateTime SentAt { get; set; } = DateTime.UtcNow;
    public bool IsRead { get; set; } = false;
    public string? MessageType { get; set; } = "text";
    
    // Navigation properties
    public User? Sender { get; set; }
    public User? Receiver { get; set; }
}
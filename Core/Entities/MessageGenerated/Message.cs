using PaymentCoreServiceApi.Core.Entities.BaseModel;
using PaymentCoreServiceApi.Core.Entities.UserGenerated;

namespace PaymentCoreServiceApi.Core.Entities.MessageGenerated;

public class Message : EntityBase
{
    public long ConversationId { get; set; }
    public long SenderId { get; set; }

    public MessageType MessageType { get; set; } = MessageType.Text;
    public string? Content { get; set; }
    public string? FileUrl { get; set; }
    public string? Metadata { get; set; } // JSON: { "width": 1920, "height": 1080 } hoặc { "size": 1024 }

}
// 1 Conversation → có thể có nhiều Message
// 1 Message → chỉ thuộc về 1 Conversation

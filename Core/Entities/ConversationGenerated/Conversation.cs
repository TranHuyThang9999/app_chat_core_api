using PaymentCoreServiceApi.Core.Entities.BaseModel;

namespace AppChat.Core.Entities.ConversationGenerated;

public class Conversation: EntityBase
{
    public string? Name { get; set; }         // null nếu là chat 1-1
    public bool IsGroup { get; set; }         // true = nhóm, false = 1-1
    public string? AvatarUrl { get; set; }    // URL ảnh đại diện
}

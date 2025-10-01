using PaymentCoreServiceApi.Core.Entities.BaseModel;

namespace PaymentCoreServiceApi.Core.Entities.ConversationMemberGenerated;

public class ChannelMember:  EntityBase
{
    public long ChannelId { get; set; }
    public long UserId { get; set; }

    public DateTime JoinedAt { get; set; } = DateTime.UtcNow;
    public bool IsAdmin { get; set; } = false;
    public bool IsLeft { get; set; } = false;
    public DateTime? LeftAt { get; set; }
}

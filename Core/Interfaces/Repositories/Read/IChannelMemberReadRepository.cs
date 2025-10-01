using PaymentCoreServiceApi.Core.Entities.ConversationMemberGenerated;

namespace PaymentCoreServiceApi.Core.Interfaces.Repositories.Read;

public interface IChannelMemberReadRepository : IBaseReadRepository<ChannelMember>
{
    Task<bool> IsUserInChannelAsync(long channelId, long userId, CancellationToken cancellationToken = default);
    Task<IEnumerable<ChannelMember>> GetChannelMembersAsync(long channelId, CancellationToken cancellationToken = default);
}
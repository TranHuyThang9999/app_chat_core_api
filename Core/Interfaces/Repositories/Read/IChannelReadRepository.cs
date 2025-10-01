using AppChat.Core.Entities.ConversationGenerated;

namespace PaymentCoreServiceApi.Core.Interfaces.Repositories.Read;

public interface IChannelReadRepository : IBaseReadRepository<Channel>
{
    Task<Channel?> GetPrivateConversationAsync(long userId1, long userId2, CancellationToken cancellationToken = default);
    Task<IEnumerable<Channel>> GetUserConversationsAsync(long userId, CancellationToken cancellationToken = default);
}

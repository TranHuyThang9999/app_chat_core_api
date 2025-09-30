using AppChat.Core.Entities.ConversationGenerated;

namespace PaymentCoreServiceApi.Core.Interfaces.Repositories.Read;

public interface IConversationReadRepository : IBaseReadRepository<Conversation>
{
    Task<Conversation?> GetPrivateConversationAsync(long userId1, long userId2, CancellationToken cancellationToken = default);
    Task<IEnumerable<Conversation>> GetUserConversationsAsync(long userId, CancellationToken cancellationToken = default);
}
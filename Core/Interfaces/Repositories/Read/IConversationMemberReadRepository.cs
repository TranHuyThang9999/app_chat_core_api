using PaymentCoreServiceApi.Core.Entities.ConversationMemberGenerated;

namespace PaymentCoreServiceApi.Core.Interfaces.Repositories.Read;

public interface IConversationMemberReadRepository : IBaseReadRepository<ConversationMember>
{
    Task<bool> IsUserInConversationAsync(long conversationId, long userId, CancellationToken cancellationToken = default);
    Task<IEnumerable<ConversationMember>> GetConversationMembersAsync(long conversationId, CancellationToken cancellationToken = default);
}
using PaymentCoreServiceApi.Core.Entities.MessageGenerated;

namespace PaymentCoreServiceApi.Core.Interfaces.Repositories.Read;

public interface IMessageReadRepository : IBaseReadRepository<Message>
{
    Task<IEnumerable<Message>> GetMessagesByConversationAsync(long conversationId, long userId,bool onlyUnread = false,int skip = 0,int take = 50,CancellationToken cancellationToken = default);
}

using PaymentCoreServiceApi.Core.Entities.MessageGenerated;

namespace PaymentCoreServiceApi.Core.Interfaces.Repositories.Read;

public interface IMessageReadRepository : IBaseReadRepository<Message>
{
    Task<IEnumerable<Message>> GetMessagesBetweenUsersAsync(long senderId, long receiverId);
    Task<IEnumerable<Message>> GetUnreadMessagesAsync(long receiverId);
    Task<IEnumerable<Message>> GetMessagesByUserAsync(long userId);
}
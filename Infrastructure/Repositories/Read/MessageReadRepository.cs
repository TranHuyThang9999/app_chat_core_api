using Microsoft.EntityFrameworkCore;
using PaymentCoreServiceApi.Core.Entities.MessageGenerated;
using PaymentCoreServiceApi.Core.Interfaces.Repositories.Read;
using PaymentCoreServiceApi.Infrastructure.DbContexts;

namespace PaymentCoreServiceApi.Infrastructure.Repositories.Read;

public class MessageReadRepository : EfBaseReadOnlyRepository<Message>, IMessageReadRepository
{
    private readonly AppDbContext _context;

    public MessageReadRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Message>> GetMessagesBetweenUsersAsync(long senderId, long receiverId)
    {
        return await _context.Messages
            .AsNoTracking()
            .Where(m => (m.SenderId == senderId && m.ReceiverId == receiverId) ||
                       (m.SenderId == receiverId && m.ReceiverId == senderId))
            .Include(m => m.Sender)
            .Include(m => m.Receiver)
            .OrderBy(m => m.SentAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<Message>> GetUnreadMessagesAsync(long receiverId)
    {
        return await _context.Messages
            .AsNoTracking()
            .Where(m => m.ReceiverId == receiverId && !m.IsRead)
            .Include(m => m.Sender)
            .OrderBy(m => m.SentAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<Message>> GetMessagesByUserAsync(long userId)
    {
        return await _context.Messages
            .AsNoTracking()
            .Where(m => m.SenderId == userId || m.ReceiverId == userId)
            .Include(m => m.Sender)
            .Include(m => m.Receiver)
            .OrderByDescending(m => m.SentAt)
            .ToListAsync();
    }
}
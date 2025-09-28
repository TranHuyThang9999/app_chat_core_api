using Microsoft.EntityFrameworkCore;
using PaymentCoreServiceApi.Core.Entities.MessageGenerated;
using PaymentCoreServiceApi.Core.Interfaces.Repositories.Write;
using PaymentCoreServiceApi.Infrastructure.DbContexts;

namespace PaymentCoreServiceApi.Infrastructure.Repositories.Write;

public class MessageWriteRepository : EfBaseWriteOnlyRepository<Message>, IMessageWriteRepository
{
    private readonly AppDbContext _context;
    public MessageWriteRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }
}
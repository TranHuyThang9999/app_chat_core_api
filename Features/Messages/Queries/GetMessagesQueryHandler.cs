using PaymentCoreServiceApi.Common;
using PaymentCoreServiceApi.Common.Mediator;
using PaymentCoreServiceApi.Core.Entities.MessageGenerated;
using PaymentCoreServiceApi.Core.Interfaces.Repositories.Read;

namespace PaymentCoreServiceApi.Features.Messages.Queries;

public class GetMessagesQueryHandler : IRequestApiResponseHandler<GetMessagesQuery, IEnumerable<Message>>
{
    private readonly IMessageReadRepository _messageReadRepository;

    public GetMessagesQueryHandler(IMessageReadRepository messageReadRepository)
    {
        _messageReadRepository = messageReadRepository;
    }

    public async Task<ApiResponse<IEnumerable<Message>>> Handle(GetMessagesQuery request, CancellationToken cancellationToken)
    {
        return null;
    }
}

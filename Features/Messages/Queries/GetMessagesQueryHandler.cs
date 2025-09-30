using PaymentCoreServiceApi.Common;
using PaymentCoreServiceApi.Common.Mediator;
using PaymentCoreServiceApi.Core.Entities.MessageGenerated;
using PaymentCoreServiceApi.Core.Interfaces.Repositories.Read;
using PaymentCoreServiceApi.Services;

namespace PaymentCoreServiceApi.Features.Messages.Queries;

public class GetMessagesQueryHandler : IRequestApiResponseHandler<GetMessagesQuery, PagedResult<Message>>
{
    private readonly IMessageReadRepository _messageReadRepository;
    private readonly IExecutionContext _currentUser;


    public GetMessagesQueryHandler(IMessageReadRepository messageReadRepository, IExecutionContext currentUser)
    {
        _messageReadRepository = messageReadRepository;
        _currentUser = currentUser;
    }

    public async Task<ApiResponse<PagedResult<Message>>> Handle(GetMessagesQuery request, CancellationToken cancellationToken)
    {
        var messages = await _messageReadRepository.GetMessagesByConversationAsync(
            request.ConversationId,
            _currentUser.Id,
            request.OnlyUnread,
            request.Skip,
            request.Take,
            cancellationToken);
        if (messages is null)
        {
            return ApiResponse<PagedResult<Message>>.NotFound("Messages not found");
        }
        return ApiResponse<PagedResult<Message>>.Success(new PagedResult<Message>(messages, request.Skip, request.Take, messages.Count()));
    }
}

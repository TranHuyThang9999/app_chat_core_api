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
        try
        {
            IEnumerable<Message> messages;

            if (request.OnlyUnread)
            {
                messages = await _messageReadRepository.GetUnreadMessagesAsync(request.UserId);
            }
            else if (request.OtherUserId.HasValue)
            {
                messages = await _messageReadRepository.GetMessagesBetweenUsersAsync(request.UserId, request.OtherUserId.Value);
            }
            else
            {
                messages = await _messageReadRepository.GetMessagesByUserAsync(request.UserId);
            }

            return ApiResponse<IEnumerable<Message>>.Success(messages, "Messages retrieved successfully");
        }
        catch (Exception ex)
        {
            return ApiResponse<IEnumerable<Message>>.InternalServerError($"Error retrieving messages: {ex.Message}");
        }
    }
}
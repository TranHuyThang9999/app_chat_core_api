using PaymentCoreServiceApi.Common;
using PaymentCoreServiceApi.Common.Mediator;
using PaymentCoreServiceApi.Core.Entities.MessageGenerated;
using PaymentCoreServiceApi.Core.Interfaces.Repositories.Read;
using PaymentCoreServiceApi.Core.Interfaces.Repositories.Write;
using PaymentCoreServiceApi.Services;

namespace PaymentCoreServiceApi.Features.Messages.Commands;

public class SendMessageCommandHandler : IRequestApiResponseHandler<SendMessageCommand, Message>
{
    private readonly IMessageWriteRepository _messageWriteRepository;
    private readonly IUserReadRepository _userReadRepository;
    private readonly IExecutionContext _currentUser;

    public SendMessageCommandHandler(
        IMessageWriteRepository messageWriteRepository,
        IUserReadRepository userReadRepository,
        IExecutionContext currentUser)
    {
        _messageWriteRepository = messageWriteRepository;
        _userReadRepository = userReadRepository;
        _currentUser = currentUser;
    }

    public async Task<ApiResponse<Message>> Handle(SendMessageCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Validate sender exists
            var sender = await _userReadRepository.GetByIdAsync(_currentUser.Id);
            if (sender == null)
            {
                return ApiResponse<Message>.NotFound("Sender not found");
            }

            // Validate receiver exists
            var receiver = await _userReadRepository.GetByIdAsync(request.ReceiverId);
            if (receiver == null)
            {
                return ApiResponse<Message>.NotFound("Receiver not found");
            }

            // Create message
            var message = new Message
            {
                SenderId = _currentUser.Id,
                ReceiverId = request.ReceiverId,
                Content = request.Content,
                SentAt = DateTime.UtcNow,
                IsRead = false
            };
            // Send message
            var sentMessage = await _messageWriteRepository.AddAsync(message);

            await _messageWriteRepository.CommitAsync();

            return ApiResponse<Message>.Success(sentMessage, "Message sent successfully");
        }
        catch (Exception ex)
        {
            return ApiResponse<Message>.InternalServerError($"Error sending message: {ex.Message}");
        }
    }
}
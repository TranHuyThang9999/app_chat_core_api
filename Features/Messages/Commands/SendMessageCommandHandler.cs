using AppChat.Core.Entities.ConversationGenerated;
using PaymentCoreServiceApi.Common;
using PaymentCoreServiceApi.Common.Mediator;
using PaymentCoreServiceApi.Core.Entities.ConversationMemberGenerated;
using PaymentCoreServiceApi.Core.Entities.MessageGenerated;
using PaymentCoreServiceApi.Core.Interfaces.Repositories.Read;
using PaymentCoreServiceApi.Core.Interfaces.Repositories.Write;
using PaymentCoreServiceApi.Services;

namespace PaymentCoreServiceApi.Features.Messages.Commands;

public class SendMessageCommandHandler : IRequestApiResponseHandler<SendMessageCommand, Message>
{
    private readonly IMessageWriteRepository _messageWriteRepository;
    private readonly IConversationWriteRepository _conversationWriteRepository;
    private readonly IUserReadRepository _userReadRepository;
    private readonly IExecutionContext _currentUser;
    private readonly ILogger<SendMessageCommandHandler> _logger;
    private readonly IConversationMemberWriteRepository _conversationMemberWriteRepository;

    public SendMessageCommandHandler(
        IMessageWriteRepository messageWriteRepository,
        IConversationWriteRepository conversationWriteRepository,
        IConversationMemberWriteRepository conversationMemberWriteRepository,
        IUserReadRepository userReadRepository,
        IExecutionContext currentUser,
        ILogger<SendMessageCommandHandler> logger)
    {
        _messageWriteRepository = messageWriteRepository;
        _conversationWriteRepository = conversationWriteRepository;
        _userReadRepository = userReadRepository;
        _currentUser = currentUser;
        _logger = logger;
        _conversationMemberWriteRepository = conversationMemberWriteRepository;
    }

    public async Task<ApiResponse<Message>> Handle(SendMessageCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var conversation = await _conversationWriteRepository.AddAsync(new Conversation());
            await _conversationMemberWriteRepository.AddRangeAsync(new List<ConversationMember>
        {
            new ConversationMember
            {
                ConversationId = conversation.Id,
                UserId = _currentUser.Id,
            },
            new ConversationMember
            {
                ConversationId = conversation.Id,
                UserId = request.ReceiverId,
            },
        });
            var message = new Message
            {
                Content = request.Content,
                CreatedAt = DateTime.UtcNow,
                SenderId = _currentUser.Id,
                ConversationId = conversation.Id,
            };
            await _messageWriteRepository.AddAsync(message);
            await _messageWriteRepository.CommitAsync(cancellationToken);
            
            return ApiResponse<Message>.Success(message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while sending message");
            throw;
        }

    }
}

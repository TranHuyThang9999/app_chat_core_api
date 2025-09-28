using PaymentCoreServiceApi.Core.Entities.BaseModel;
using PaymentCoreServiceApi.Core.Entities.UserGenerated;

namespace PaymentCoreServiceApi.Core.Entities.MessageGenerated;

public class Message : EntityBase
{
    public long SenderId { get; set; }
    public long ReceiverId { get; set; }
    public string Content { get; set; } = string.Empty;
    public DateTime SentAt { get; set; } = DateTime.UtcNow;
    public bool IsRead { get; set; } = false;
    public string? MessageType { get; set; } = "text";
    
    // Navigation properties
    public User? Sender { get; set; }
    public User? Receiver { get; set; }
}

// using System;
// using System.Collections.Generic;

// namespace PaymentCoreServiceApi.Core.Entities.Chat
// {
//     public class User
//     {
//         public long Id { get; set; }
//         public string UserName { get; set; } = default!;
//         public string Email { get; set; } = default!;

//         public ICollection<Message> Messages { get; set; } = new List<Message>();
//         public ICollection<MessageReaction> Reactions { get; set; } = new List<MessageReaction>();
//         public ICollection<ConversationMember> Conversations { get; set; } = new List<ConversationMember>();
//     }

//     public class Conversation
//     {
//         public long Id { get; set; }
//         public string Name { get; set; } = default!; // group chat name (nullable if 1-1)
//         public bool IsGroup { get; set; }

//         public ICollection<ConversationMember> Members { get; set; } = new List<ConversationMember>();
//         public ICollection<Message> Messages { get; set; } = new List<Message>();
//     }

//     public class ConversationMember
//     {
//         public long ConversationId { get; set; }
//         public Conversation Conversation { get; set; } = default!;

//         public long UserId { get; set; }
//         public User User { get; set; } = default!;

//         public DateTime JoinedAt { get; set; } = DateTime.UtcNow;
//         public bool IsAdmin { get; set; }
//     }

//     public enum MessageType
//     {
//         Text = 1,
//         Image = 2,
//         File = 3,
//         System = 4
//     }

//     public class Message
//     {
//         public long Id { get; set; }
//         public long ConversationId { get; set; }
//         public Conversation Conversation { get; set; } = default!;

//         public long SenderId { get; set; }
//         public User Sender { get; set; } = default!;

//         public MessageType MessageType { get; set; }
//         public string? Content { get; set; } // Text message
//         public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

//         public ICollection<MessageAttachment> Attachments { get; set; } = new List<MessageAttachment>();
//         public ICollection<MessageReaction> Reactions { get; set; } = new List<MessageReaction>();
//     }

//     public enum FileType
//     {
//         Image,
//         Video,
//         Audio,
//         Document,
//         Other
//     }

//     public class MessageAttachment
//     {
//         public long Id { get; set; }
//         public long MessageId { get; set; }
//         public Message Message { get; set; } = default!;

//         public string FileName { get; set; } = default!;
//         public string FileUrl { get; set; } = default!;
//         public FileType FileType { get; set; }
//         public long FileSize { get; set; }
//     }

//     public class MessageReaction
//     {
//         public long Id { get; set; }
//         public long MessageId { get; set; }
//         public Message Message { get; set; } = default!;

//         public long UserId { get; set; }
//         public User User { get; set; } = default!;

//         public string ReactionType { get; set; } = default!; // "👍", "❤️", "😂"
//         public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
//     }
// }

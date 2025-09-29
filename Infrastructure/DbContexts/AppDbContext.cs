using AppChat.Core.Entities.ConversationGenerated;
using Microsoft.EntityFrameworkCore;
using PaymentCoreServiceApi.Core.Entities.BankAccountGenerated;
using PaymentCoreServiceApi.Core.Entities.ConversationMemberGenerated;
using PaymentCoreServiceApi.Core.Entities.MessageGenerated;
using PaymentCoreServiceApi.Core.Entities.UserGenerated;

namespace PaymentCoreServiceApi.Infrastructure.DbContexts;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<BankAccount> BankAccounts { get; set; }
    /// <summary>
    /// Bảng lưu các tin nhắn (Message).
    /// - Mỗi tin nhắn thuộc về một Conversation.
    /// - Ghi lại nội dung, người gửi, thời gian gửi.
    /// - Hỗ trợ mở rộng: kiểu tin (text, ảnh, file), trạng thái đọc, phản hồi...
    /// </summary>
    public DbSet<Message> Messages { get; set; }

    /// <summary>
    /// Bảng lưu các cuộc hội thoại (Conversation).
    /// - Đại diện cho "phòng chat": có thể là chat 1-1 hoặc nhóm.
    /// - Chứa tên, ảnh đại diện, loại (IsGroup), v.v.
    /// - Là nơi tập hợp tất cả tin nhắn và thành viên.
    /// </summary>
    public DbSet<Conversation> Conversations { get; set; }

    /// <summary>
    /// Bảng trung gian quản lý thành viên trong hội thoại (ConversationMember).
    /// - Thiết lập mối quan hệ nhiều-nhiều giữa User và Conversation.
    /// - Theo dõi: ai tham gia, khi nào, có phải admin không, đã rời nhóm chưa.
    /// - Cốt lõi để kiểm soát quyền và hiển thị danh sách thành viên.
    /// </summary>
    public DbSet<ConversationMember> ConversationMembers { get; set; }

    // Để cập nhật cơ sở dữ liệu, chạy lệnh:
    // dotnet ef database update
}


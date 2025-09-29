namespace PaymentCoreServiceApi.Core.Entities.MessageReceiptGenerated;
// Bảng quản lý trạng thái đọc tin nhắn theo người dùng
public class MessageReceipt
{
    public long MessageId { get; set; }
    public long UserId { get; set; }
    public bool IsRead { get; set; } = false;
    public DateTime? ReadAt { get; set; }
}

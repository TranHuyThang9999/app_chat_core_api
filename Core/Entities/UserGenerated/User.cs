using PaymentCoreServiceApi.Core.Entities.BaseModel;

namespace PaymentCoreServiceApi.Core.Entities.UserGenerated;

public class User : EntityBase
{
    public string? NickName { get; set; }
    public string Avatar { get; set; }
    public int? Gender { get; set; }
    public DateTime? BirthDate { get; set; }
    public int Age { get; set; }
    public string Email { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public string PhoneNumber { get; set; }
    public string Address { get; set; }
}
using System;
using System.Collections.Generic;

namespace PaymentCoreServiceApi.Generate.ModelsGenerate;

public partial class User
{
    public long Id { get; set; }

    public string NickName { get; set; } = null!;

    public string Avatar { get; set; } = null!;

    public int? Gender { get; set; }

    public DateTime? BirthDate { get; set; }

    public int Age { get; set; }

    public string Email { get; set; } = null!;

    public string UserName { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;

    public string Address { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public bool Active { get; set; }

    public bool Deleted { get; set; }

    public int? DeletedBy { get; set; }

    public DateTime? DeletedAt { get; set; }

    public int? CreatedBy { get; set; }

    public int? UpdatedBy { get; set; }
}

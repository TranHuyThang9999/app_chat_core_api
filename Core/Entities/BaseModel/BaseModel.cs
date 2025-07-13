namespace PaymentCoreServiceApi.Core.Entities.BaseModel;

public abstract class EntityBase
{
    public long Id { get; private set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public bool Active { get; set; } = true;
    public bool Deleted { get; set; } = false;
    public int? DeletedBy { get; set; } = null;
    public DateTime? DeletedAt { get; set; } = null;
    public long CreatedBy { get; set; } = 0;
    public long UpdatedBy { get; set; } = 0;
}
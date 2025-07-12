namespace PaymentCoreServiceApi.Core.Entities.BaseModel;

public abstract class EntityBase
{
    public long Id { get; private set; } = Random.Shared.NextInt64(1000, 10000);
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public bool Active { get; set; }
    public bool Deleted { get; set; }
    public int? DeletedBy { get; set; }
    public DateTime? DeletedAt { get; set; }
    public int? CreatedBy { get; set; }
    public int? UpdatedBy { get; set; }
}
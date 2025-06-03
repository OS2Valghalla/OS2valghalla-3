using Valghalla.Database.Interceptors.ChangeTracking;

namespace Valghalla.Database.Entities.Tables;

public partial class WorkLocationTemplateEntity : IChangeTrackingEntity
{
    public Guid Id { get; set; }
    public Guid AreaId { get; set; }
    public string Title { get; set; } = null!;
    public string Address { get; set; } = null!;
    public string PostalCode { get; set; } = null!;
    public string City { get; set; } = null!;
    public int VoteLocation { get; set; }
    public DateTime CreatedAt { get; set; }
    public Guid CreatedBy { get; set; }
    public DateTime? ChangedAt { get; set; }
    public Guid? ChangedBy { get; set; }

    public virtual AreaEntity Area { get; set; } = null!;
    public virtual UserEntity CreatedByUser { get; set; } = null!;
    public virtual UserEntity? ChangedByUser { get; set; }
}

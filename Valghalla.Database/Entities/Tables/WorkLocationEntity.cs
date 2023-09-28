using Valghalla.Database.Interceptors.Audit;
using Valghalla.Database.Interceptors.ChangeTracking;

namespace Valghalla.Database.Entities.Tables;

public partial class WorkLocationEntity : IChangeTrackingEntity
{ 
    public Guid Id { get; set; }
    public Guid AreaId { get; set; }
    public string Title { get; set; } = null!;
    public string Address { get; set; } = null!;
    public string PostalCode { get; set; } = null!;
    public string City { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public Guid CreatedBy { get; set; }
    public DateTime? ChangedAt { get; set; }
    public Guid? ChangedBy { get; set; }

    public virtual ICollection<WorkLocationResponsibleEntity> WorkLocationResponsibles { get; } = new List<WorkLocationResponsibleEntity>();
    public virtual ICollection<WorkLocationTaskTypeEntity> WorkLocationTaskTypes { get; } = new List<WorkLocationTaskTypeEntity>();
    public virtual ICollection<WorkLocationTeamEntity> WorkLocationTeams { get; } = new List<WorkLocationTeamEntity>();
    public virtual AreaEntity Area { get; set; } = null!;
    public virtual UserEntity CreatedByUser { get; set; } = null!;
    public virtual UserEntity? ChangedByUser { get; set; }
    public virtual ICollection<ElectionEntity> Elections { get; } = new List<ElectionEntity>();
    public virtual ICollection<ElectionWorkLocationEntity> ElectionWorkLocations { get; } = new List<ElectionWorkLocationEntity>();
}

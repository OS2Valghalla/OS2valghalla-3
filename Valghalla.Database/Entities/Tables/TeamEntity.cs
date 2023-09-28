using Valghalla.Database.Interceptors.ChangeTracking;

namespace Valghalla.Database.Entities.Tables;

public partial class TeamEntity : IChangeTrackingEntity 
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string ShortName { get; set; } = null!;
    public string? Description { get; set; }

    public DateTime CreatedAt { get; set; }
    public Guid CreatedBy { get; set; }
    public DateTime? ChangedAt { get; set; }
    public Guid? ChangedBy { get; set; }

    public virtual UserEntity CreatedByUser { get; set; } = null!;
    public virtual UserEntity? ChangedByUser { get; set; }
    public virtual ICollection<WorkLocationTeamEntity> WorkLocationTeams { get; } = new List<WorkLocationTeamEntity>();
    public virtual ICollection<ParticipantEntity> ResponsibleParticipants { get; } = new List<ParticipantEntity>();
    public virtual ICollection<TeamResponsibleEntity> TeamResponsibles { get; } = new List<TeamResponsibleEntity>();
    public virtual ICollection<ParticipantEntity> MemberParticipants { get; } = new List<ParticipantEntity>();
    public virtual ICollection<TeamMemberEntity> TeamMembers { get; } = new List<TeamMemberEntity>();
}

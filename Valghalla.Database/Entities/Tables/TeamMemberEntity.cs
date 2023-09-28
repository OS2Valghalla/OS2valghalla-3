using Valghalla.Database.Interceptors.ChangeTracking;

namespace Valghalla.Database.Entities.Tables
{
    public partial class TeamMemberEntity : IChangeTrackingEntity
    {
        public Guid TeamId { get; set; }
        public Guid ParticipantId { get; set; }

        public DateTime CreatedAt { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime? ChangedAt { get; set; }
        public Guid? ChangedBy { get; set; }

        public virtual UserEntity CreatedByUser { get; set; } = null!;
        public virtual UserEntity? ChangedByUser { get; set; }
        public virtual TeamEntity Team { get; set; } = null!;
        public virtual ParticipantEntity Participant { get; set; } = null!;
    }
}

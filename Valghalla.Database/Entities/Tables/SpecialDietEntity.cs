using Valghalla.Database.Interceptors.ChangeTracking;

namespace Valghalla.Database.Entities.Tables
{
    public partial class SpecialDietEntity : IChangeTrackingEntity
    {
        public Guid Id { get; set; }
        public string? Title { get; set; }

        public DateTime CreatedAt { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime? ChangedAt { get; set; }
        public Guid? ChangedBy { get; set; }

        public virtual UserEntity CreatedByUser { get; set; } = null!;
        public virtual UserEntity? ChangedByUser { get; set; }

        public virtual ICollection<ParticipantEntity> Participants { get; } = new List<ParticipantEntity>();
        public virtual ICollection<SpecialDietParticipantEntity> SpecialDietParticipants { get; } = new List<SpecialDietParticipantEntity>();
    }
}

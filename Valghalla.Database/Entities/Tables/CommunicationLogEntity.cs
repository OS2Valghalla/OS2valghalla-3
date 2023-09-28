using Valghalla.Database.Interceptors.ChangeTracking;

namespace Valghalla.Database.Entities.Tables
{
    public partial class CommunicationLogEntity : IChangeTrackingEntity
    {
        public Guid Id { get; set; }
        public Guid ParticipantId { get; set; }
        public int MessageType { get; set; }
        public int SendType { get; set; }
        public string Message { get; set; } = null!;
        public string ShortMessage { get; set; } = null!;
        public int Status { get; set; }
        public string? Error { get; set; }

        public DateTime CreatedAt { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime? ChangedAt { get; set; }
        public Guid? ChangedBy { get; set; }

        public virtual ParticipantEntity Participant { get; set; } = null!;
        public virtual UserEntity CreatedByUser { get; set; } = null!;
        public virtual UserEntity? ChangedByUser { get; set; }
    }
}

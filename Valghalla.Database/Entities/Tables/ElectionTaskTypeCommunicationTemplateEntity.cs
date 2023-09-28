using Valghalla.Database.Interceptors.ChangeTracking;

namespace Valghalla.Database.Entities.Tables
{
    public partial class ElectionTaskTypeCommunicationTemplateEntity: IChangeTrackingEntity
    {
        public Guid ElectionId { get; set; }
        public Guid TaskTypeId { get; set; }
        public Guid? ConfirmationOfRegistrationCommunicationTemplateId { get; set; }
        public Guid? ConfirmationOfCancellationCommunicationTemplateId { get; set; }
        public Guid? InvitationCommunicationTemplateId { get; set; }
        public Guid? InvitationReminderCommunicationTemplateId { get; set; }
        public Guid? TaskReminderCommunicationTemplateId { get; set; }
        public Guid? RetractedInvitationCommunicationTemplateId { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime? ChangedAt { get; set; }
        public Guid? ChangedBy { get; set; }
        public virtual UserEntity CreatedByUser { get; set; } = null!;
        public virtual UserEntity? ChangedByUser { get; set; }
        public virtual ElectionEntity Election { get; set; } = null!;
        public virtual TaskTypeEntity TaskType { get; set; } = null!;
        public virtual CommunicationTemplateEntity? ConfirmationOfRegistrationCommunicationTemplate { get; set; }
        public virtual CommunicationTemplateEntity? ConfirmationOfCancellationCommunicationTemplate { get; set; }
        public virtual CommunicationTemplateEntity? InvitationCommunicationTemplate { get; set; }
        public virtual CommunicationTemplateEntity? InvitationReminderCommunicationTemplate { get; set; }
        public virtual CommunicationTemplateEntity? TaskReminderCommunicationTemplate { get; set; }
        public virtual CommunicationTemplateEntity? RetractedInvitationCommunicationTemplate { get; set; }
    }
}

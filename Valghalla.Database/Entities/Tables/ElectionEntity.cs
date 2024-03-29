﻿using Valghalla.Database.Interceptors.ChangeTracking;

namespace Valghalla.Database.Entities.Tables;

public partial class ElectionEntity : IChangeTrackingEntity
{
    public Guid Id { get; set; }
    public Guid ElectionTypeId { get; set; }
    public string Title { get; set; } = null!;
    public int LockPeriod { get; set; }
    public DateTime ElectionStartDate { get; set; }
    public DateTime ElectionEndDate { get; set; }
    public DateTime ElectionDate { get; set; }
    public bool Active { get; set; }
    public Guid? ConfirmationOfRegistrationCommunicationTemplateId { get; set; }
    public Guid? ConfirmationOfCancellationCommunicationTemplateId { get; set; }
    public Guid? InvitationCommunicationTemplateId { get; set; }
    public Guid? InvitationReminderCommunicationTemplateId { get; set; }
    public Guid? TaskReminderCommunicationTemplateId { get; set; }
    public Guid? RetractedInvitationCommunicationTemplateId { get; set; }
    public Guid? RemovedFromTaskCommunicationTemplateId { get; set; }
    public Guid? RemovedByValidationCommunicationTemplateId { get; set; }

    public DateTime CreatedAt { get; set; }
    public Guid CreatedBy { get; set; }
    public DateTime? ChangedAt { get; set; }
    public Guid? ChangedBy { get; set; }

    public virtual UserEntity CreatedByUser { get; set; } = null!;
    public virtual UserEntity? ChangedByUser { get; set; }

    public virtual ElectionTypeEntity ElectionType { get; set; } = null!;
    public virtual ICollection<WorkLocationEntity> WorkLocations { get; } = new List<WorkLocationEntity>();
    public virtual ICollection<ElectionWorkLocationEntity> ElectionWorkLocations { get; } = new List<ElectionWorkLocationEntity>();
    public virtual CommunicationTemplateEntity? ConfirmationOfRegistrationCommunicationTemplate { get; set; }
    public virtual CommunicationTemplateEntity? ConfirmationOfCancellationCommunicationTemplate { get; set; }
    public virtual CommunicationTemplateEntity? InvitationCommunicationTemplate { get; set; }
    public virtual CommunicationTemplateEntity? InvitationReminderCommunicationTemplate { get; set; }
    public virtual CommunicationTemplateEntity? TaskReminderCommunicationTemplate { get; set; }
    public virtual CommunicationTemplateEntity? RetractedInvitationCommunicationTemplate { get; set; }
    public virtual CommunicationTemplateEntity? RemovedFromTaskCommunicationTemplate { get; set; }
    public virtual CommunicationTemplateEntity? RemovedByValidationCommunicationTemplate { get; set; }
    public virtual ICollection<ElectionTaskTypeCommunicationTemplateEntity> ElectionTaskTypeCommunicationTemplates { get; } = new List<ElectionTaskTypeCommunicationTemplateEntity>();
}

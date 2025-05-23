namespace Valghalla.Application.Communication
{
    public interface ICommunicationQueryRepository
    {
        Task<TaskAssignmentCommunicationInfo?> GetTaskAssignmentCommunicationInfoAsync(Guid taskAssignmentId, CancellationToken cancellationToken);
        Task<CommunicationRelatedInfo?> GetCommunicationRelatedInfoAsync(Guid participantId, Guid taskAssignmentId, CancellationToken cancellationToken);
        Task<CommunicationRelatedInfo?> GetRejectedTaskInfoAsync(Guid rejectedTaskId, CancellationToken cancellationToken);
        Task<CommunicationTemplate?> GetTaskInvitationCommunicationTemplateAsync(Guid taskAssignmentId, CancellationToken cancellationToken);
        Task<CommunicationTemplate?> GetRemovedFromTaskCommunicationTemplateAsync(Guid taskAssignmentId, CancellationToken cancellationToken);
        Task<CommunicationTemplate?> GetRemovedFromTaskByValidationCommunicationTemplateAsync(Guid taskAssignmentId, CancellationToken cancellationToken);
        Task<CommunicationTemplate?> GetTaskRegistrationCommunicationTemplateAsync(Guid taskAssignmentId, CancellationToken cancellationToken);
        Task<CommunicationTemplate?> GetTaskCancellationCommunicationTemplateAsync(Guid taskAssignmentId, CancellationToken cancellationToken);
        Task<CommunicationTemplate?> GetTaskInvitationReminderCommunicationTemplateAsync(Guid taskAssignmentId, CancellationToken cancellationToken);
        Task<CommunicationTemplate?> GetTaskReminderCommunicationTemplateAsync(Guid taskAssignmentId, CancellationToken cancellationToken);
        Task<CommunicationTemplate?> GetTaskRetractedInvitationCommunicationTemplateAsync(Guid taskAssignmentId, CancellationToken cancellationToken);
        Task<CommunicationTemplate?> GetCommunicationTemplateAsync(Guid templateId, CancellationToken cancellationToken);
        Task<CommunicationParticipantInfo> GetParticipantAsync(Guid participantId, CancellationToken cancellationToken);
    }
}

namespace Valghalla.Application.Communication
{
    public interface ICommunicationService
    {
        Task SendTaskInvitationAsync(Guid participantId, Guid taskAssignmentId, CancellationToken cancellationToken);
        Task SendRemovedFromTaskByValidationAsync(Guid participantId, Guid taskAssignmentId, CancellationToken cancellationToken);
        Task SendRemovedFromTaskAsync(Guid participantId, Guid taskAssignmentId, CancellationToken cancellationToken);
        Task SendTaskRegistrationAsync(Guid participantId, Guid taskAssignmentId, CancellationToken cancellationToken);
        Task SendTaskCancellationAsync(Guid participantId, Guid taskAssignmentId, CancellationToken cancellationToken);
        Task SendTaskInvitationRetractedAsync(Guid participantId, Guid taskAssignmentId, CancellationToken cancellationToken);
        Task SendTaskInvitationReminderAsync(Guid participantId, Guid taskAssignmentId, DateTime? invitationDate, DateTime? invitationReminderDate, DateTime taskDate, CancellationToken cancellationToken);
        Task SendTaskReminderAsync(Guid participantId, Guid taskAssignmentId, DateTime? reminderDate, DateTime taskDate, CancellationToken cancellationToken);
        Task SendGroupMessageAsync(IList<CommunicationTaskParticipantInfo> tasks, int templateType, string templateSubject, string templateContent, List<Guid> fileReferenceIds, CancellationToken cancellationToken);
    }
}

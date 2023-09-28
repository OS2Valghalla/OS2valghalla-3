namespace Valghalla.Application.Communication
{
    public interface ICommunicationService
    {
        Task SendTaskInvitationAsync(Guid participantId, Guid taskAssignmentId, CancellationToken cancellationToken);
        Task SendTaskRegistrationAsync(Guid participantId, Guid taskAssignmentId, CancellationToken cancellationToken);
        Task SendTaskCancellationAsync(Guid participantId, Guid taskAssignmentId, CancellationToken cancellationToken);
        Task SendGroupMessageAsync(IList<CommunicationTaskParticipantInfo> tasks, int templateType, string templateSubject, string templateContent, List<Guid> fileReferenceIds, CancellationToken cancellationToken);
    }
}

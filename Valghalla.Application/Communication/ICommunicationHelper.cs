namespace Valghalla.Application.Communication
{
    public interface ICommunicationHelper
    {
        string ReplaceTokens(string template, CommunicationRelatedInfo info, bool htmlFormatLinks);
        Task<bool> ValidateTaskInvitationAsync(Guid participantId, Guid taskAssignmentId, CancellationToken cancellationToken);
        Task<bool> ValidateRemovedFromTaskAsync(Guid participantId, Guid taskAssignmentId, CancellationToken cancellationToken);
        Task<bool> ValidateRemovedFromTaskByValidationAsync(Guid participantId, Guid taskAssignmentId, CancellationToken cancellationToken);
        Task<bool> ValidateTaskInvitationReminderAsync(Guid participantId, Guid taskAssignmentId, CancellationToken cancellationToken);
        Task<bool> ValidateTaskReminderAsync(Guid participantId, Guid taskAssignmentId, CancellationToken cancellationToken);
        Task<bool> ValidateTaskRegistrationAsync(Guid participantId, Guid taskAssignmentId, CancellationToken cancellationToken);
        Task<bool> ValidateTaskInvitationRetractedAsync(Guid taskAssignmentId, CancellationToken cancellationToken);
    }
}

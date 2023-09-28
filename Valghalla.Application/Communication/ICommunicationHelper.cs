namespace Valghalla.Application.Communication
{
    public interface ICommunicationHelper
    {
        string ReplaceTokens(string template, CommunicationRelatedInfo info);
        Task<bool> ValidateTaskInvitationAsync(Guid participantId, Guid taskAssignmentId, CancellationToken cancellationToken);
        Task<bool> ValidateTaskRegistrationAsync(Guid participantId, Guid taskAssignmentId, CancellationToken cancellationToken);
    }
}

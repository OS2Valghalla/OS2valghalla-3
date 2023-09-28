namespace Valghalla.Application.Communication
{
    public interface ICommunicationLogRepository
    {
        Task<Guid> SetCommunicationLogAsync(Guid participantId, CommunicationLogMessageType messageType, CommunicationLogSendType sendType, string message, string shortMessage, CancellationToken cancellationToken);
        Task UpdateCommunicationLogSuccessAsync(Guid communicationLogId, string message, string shortMessage, CancellationToken cancellationToken);
        Task UpdateCommunicationLogErrorAsync(Guid communicationLogId, string error, CancellationToken cancellationToken);
        Task UpdateCommunicationLogErrorAsync(Guid communicationLogId, string message, string shortMessage, string error, CancellationToken cancellationToken);
    }
}

namespace Valghalla.Worker.Infrastructure.Models
{
    public sealed record JobConfiguration
    {
        public string ConnectionString { get; init; } = null!;
        public ParticipantSyncJobConfiguration ParticipantSyncJob { get; init; } = null!;
        public TaskInvitationReminderJobConfiguration TaskInvitationReminderJob { get; init; } = null!;
        public TaskReminderJobConfiguration TaskReminderJob { get; init; } = null!;
        public ElectionActivationJobConfiguration ElectionActivationJob { get; init; } = null!;
        public ElectionDeactivationJobConfiguration ElectionDeactivationJob { get; init; } = null!;
        public TaskNotificationJobConfiguration TaskNotificationJob { get; init; } = null!;
        public AuditLogClearJobConfiguration AuditLogClearJob { get; init; } = null!;
        public CommunicationLogClearJobConfiguration CommunicationLogClearJob { get; init; } = null!;
    }

    public sealed record ParticipantSyncJobConfiguration
    {
        public int ConcurrentLimit { get; init; }
        public int Timeout { get; init; }
        public int BatchSize { get; init; }
        public int Period { get; init; }
        public int Retry { get; init; }
        public int RetryPeriod { get; init; }
    }

    public sealed record TaskInvitationReminderJobConfiguration
    {
        public int ConcurrentLimit { get; init; }
        public int Timeout { get; init; }
        public int Period { get; init; }
        public int Retry { get; init; }
        public int RetryPeriod { get; init; }
    }

    public sealed record TaskReminderJobConfiguration
    {
        public int ConcurrentLimit { get; init; }
        public int Timeout { get; init; }
        public int Period { get; init; }
        public int Retry { get; init; }
        public int RetryPeriod { get; init; }
    }

    public sealed record ElectionActivationJobConfiguration
    {
        public int ConcurrentLimit { get; init; }
        public int Timeout { get; init; }
        public int Period { get; init; }
        public int Retry { get; init; }
        public int RetryPeriod { get; init; }
    }

    public sealed record ElectionDeactivationJobConfiguration
    {
        public int ConcurrentLimit { get; init; }
        public int Timeout { get; init; }
        public int Period { get; init; }
        public int Retry { get; init; }
        public int RetryPeriod { get; init; }
    }

    public sealed record TaskNotificationJobConfiguration
    {
        public int ConcurrentLimit { get; init; }
        public int Timeout { get; init; }
        public int Retry { get; init; }
        public int RetryPeriod { get; init; }
    }

    public sealed record AuditLogClearJobConfiguration
    {
        public int MaxAvailableTime { get; init; }
        public int ConcurrentLimit { get; init; }
        public int Timeout { get; init; }
        public int Period { get; init; }
        public int Retry { get; init; }
        public int RetryPeriod { get; init; }
    }

    public sealed record CommunicationLogClearJobConfiguration
    {
        public int MaxAvailableTime { get; init; }
        public int ConcurrentLimit { get; init; }
        public int Timeout { get; init; }
        public int Period { get; init; }
        public int Retry { get; init; }
        public int RetryPeriod { get; init; }
    }
}

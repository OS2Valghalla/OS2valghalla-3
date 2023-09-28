namespace Valghalla.Application.AuditLog
{
    public interface IAuditLogService
    {
        Task AddAuditLogAsync(AuditLogBase auditLog, CancellationToken cancellationToken);
        Task AddAuditLogsAsync(IEnumerable<AuditLogBase> auditLogs, CancellationToken cancellationToken);
    }
}

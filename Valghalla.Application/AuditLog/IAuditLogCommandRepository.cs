namespace Valghalla.Application.AuditLog
{
    public interface IAuditLogCommandRepository
    {
        Task AddAuditLogAsync(AuditLogBase auditLog, Guid userId, DateTime eventDate, CancellationToken cancellationToken);
        Task AddAuditLogsAsync(IEnumerable<AuditLogBase> items, Guid userId, DateTime eventDate, CancellationToken cancellationToken);
    }
}

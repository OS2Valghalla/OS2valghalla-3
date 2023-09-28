using Valghalla.Application.AuditLog;
using Valghalla.Application.User;

namespace Valghalla.Integration.AuditLog
{
    internal class AuditLogService : IAuditLogService
    {
        private readonly IUserContextProvider userContextProvider;
        private readonly IAuditLogCommandRepository auditLogCommandRepository;

        public AuditLogService(IUserContextProvider userContextProvider, IAuditLogCommandRepository auditLogCommandRepository)
        {
            this.userContextProvider = userContextProvider;
            this.auditLogCommandRepository = auditLogCommandRepository;
        }

        public async Task AddAuditLogAsync(AuditLogBase auditLog, CancellationToken cancellationToken)
        {
            var userId = userContextProvider.CurrentUser.UserId;
            var eventDate = DateTime.UtcNow;

            await auditLogCommandRepository.AddAuditLogAsync(auditLog, userId, eventDate, cancellationToken);
        }

        public async Task AddAuditLogsAsync(IEnumerable<AuditLogBase> auditLogs, CancellationToken cancellationToken)
        {
            var userId = userContextProvider.CurrentUser.UserId;
            var eventDate = DateTime.UtcNow;

            await auditLogCommandRepository.AddAuditLogsAsync(auditLogs, userId, eventDate, cancellationToken);
        }
    }
}

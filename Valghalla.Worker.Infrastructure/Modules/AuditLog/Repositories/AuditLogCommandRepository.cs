using Microsoft.EntityFrameworkCore;
using Valghalla.Database;

namespace Valghalla.Worker.Infrastructure.Modules.AuditLog.Repositories
{
    public interface IAuditLogCommandRepository
    {
        Task ClearAuditLogsAsync(DateTime time, CancellationToken cancellationToken);
    }

    internal class AuditLogCommandRepository : IAuditLogCommandRepository
    {
        private readonly DataContext dataContext;

        public AuditLogCommandRepository(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        public async Task ClearAuditLogsAsync(DateTime time, CancellationToken cancellationToken)
        {
            await dataContext.AuditLogs
                .Where(t => t.EventDate < time)
                .ExecuteDeleteAsync(cancellationToken);
        }
    }
}

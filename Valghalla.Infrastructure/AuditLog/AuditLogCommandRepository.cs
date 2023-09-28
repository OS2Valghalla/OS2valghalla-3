using Microsoft.EntityFrameworkCore;
using Valghalla.Application.AuditLog;
using Valghalla.Database;
using Valghalla.Database.Entities.Tables;

namespace Valghalla.Infrastructure.AuditLog
{
    internal class AuditLogCommandRepository : IAuditLogCommandRepository
    {
        private readonly DataContext dataContext;
        private readonly DbSet<AuditLogEntity> auditLogs;

        public AuditLogCommandRepository(DataContext dataContext)
        {
            this.dataContext = dataContext;
            auditLogs = dataContext.Set<AuditLogEntity>();
        }

        public async Task AddAuditLogAsync(AuditLogBase auditLog, Guid userId, DateTime eventDate, CancellationToken cancellationToken)
        {
            var entity = new AuditLogEntity()
            {
                Id = Guid.NewGuid(),
                DoneBy = userId,
                EventDate = eventDate,
                Pk2name = auditLog.Pk2name,
                Pk2value = auditLog.Pk2value,
                Col2name = auditLog.Col2name,
                Col2value = auditLog.Col2value,
                Col3name = auditLog.Col3name,
                Col3value = auditLog.Col3value,
                EventTable = auditLog.EventTable,
                EventType = auditLog.EventType,
                EventDescription = auditLog.EventDescription,
            };

            await auditLogs.AddAsync(entity, cancellationToken);
            await dataContext.SaveChangesAsync(cancellationToken);
        }

        public async Task AddAuditLogsAsync(IEnumerable<AuditLogBase> items, Guid userId, DateTime eventDate, CancellationToken cancellationToken)
        {
            var entities = items.Select(auditLog => new AuditLogEntity()
            {
                Id = Guid.NewGuid(),
                DoneBy = userId,
                EventDate = eventDate,
                Pk2name = auditLog.Pk2name,
                Pk2value = auditLog.Pk2value,
                Col2name = auditLog.Col2name,
                Col2value = auditLog.Col2value,
                Col3name = auditLog.Col3name,
                Col3value = auditLog.Col3value,
                EventTable = auditLog.EventTable,
                EventType = auditLog.EventType,
                EventDescription = auditLog.EventDescription,
            });

            await auditLogs.AddRangeAsync(entities, cancellationToken);
            await dataContext.SaveChangesAsync(cancellationToken);
        }
    }
}

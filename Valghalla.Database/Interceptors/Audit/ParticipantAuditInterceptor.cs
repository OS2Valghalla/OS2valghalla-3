using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Valghalla.Application.AuditLog;
using Valghalla.Application.User;
using Valghalla.Database.Entities.Tables;
using Valghalla.Database.Interceptors.ChangeTracking;

namespace Valghalla.Database.Interceptors.Audit
{
    internal class ParticipantAuditInterceptor : SaveChangesInterceptor
    {
        private readonly IUserContextProvider userContextProvider;

        public ParticipantAuditInterceptor(IUserContextProvider userContextProvider)
        {
            this.userContextProvider = userContextProvider;
        }

        public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            if (eventData.Context != null)
            {
                await EnsureAuditLogAsync(eventData, cancellationToken);
            }

            return await base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        private async Task EnsureAuditLogAsync(DbContextEventData eventData, CancellationToken cancellationToken)
        {
            var dbContext = eventData.Context!;
            var trackingEntries = dbContext.ChangeTracker.Entries<ParticipantEntity>();

            var addedEntries = trackingEntries.Where(x => x.State == EntityState.Added);
            var modifiedEntries = trackingEntries.Where(x => x.State == EntityState.Modified);
            var deletedEntries = trackingEntries.Where(x => x.State == EntityState.Deleted);

            var auditLogsForAddedEntries = addedEntries.Select(CreateAuditLogEntityForAddedEntry);
            var auditLogsForModifiedEntries = modifiedEntries.Select(CreateAuditLogEntityForModifiedEntry);
            var auditLogsForDeletedEntries = deletedEntries.Select(CreateAuditLogEntityForDeletedEntry);

            var auditLogEntities = new List<AuditLogEntity>();
            auditLogEntities.AddRange(auditLogsForAddedEntries);
            auditLogEntities.AddRange(auditLogsForModifiedEntries);
            auditLogEntities.AddRange(auditLogsForDeletedEntries);

            if (!auditLogEntities.Any())
            {
                await Task.CompletedTask;
                return;
            }

            await dbContext.AddRangeAsync(auditLogEntities, cancellationToken);
        }

        private static AuditLogEntity CreateAuditLogEntityForAddedEntry(EntityEntry<ParticipantEntity> entityEntry)
        {
            var auditInfo = GetAuditInfo(entityEntry);
            var auditLogEntity = new AuditLogEntity()
            {
                Id = Guid.NewGuid(),
                Pk2name = auditInfo.Pk2name,
                Pk2value = auditInfo.Pk2value,
                Col2name = auditInfo.Col2name,
                Col2value = auditInfo.Col2value,
                Col3name = auditInfo.Col3name,
                Col3value = auditInfo.Col3value,
                DoneBy = entityEntry.Property(p => p.CreatedBy).CurrentValue,
                EventDate = entityEntry.Property(p => p.CreatedAt).CurrentValue,
                EventTable = auditInfo.EventTable,
                EventType = entityEntry.State.ToAuditLogEventType(),
                EventDescription = entityEntry.State.ToString(),
            };

            return auditLogEntity;
        }

        private static AuditLogEntity CreateAuditLogEntityForModifiedEntry(EntityEntry<ParticipantEntity> entityEntry)
        {
            var auditInfo = GetAuditInfo(entityEntry);
            var auditableEntityProperties = typeof(IChangeTrackingEntity).GetProperties().Select(p => p.Name);

            var changedProperties = entityEntry.Properties
                .Where(prop =>
                    ((prop.OriginalValue is not null && !prop.OriginalValue.Equals(prop.CurrentValue)) ||
                    (prop.OriginalValue is null && prop.CurrentValue is not null)) &&
                    !auditableEntityProperties.Contains(prop.Metadata.PropertyInfo!.Name))
                .Select(prop => prop.Metadata.GetColumnName());

            var auditLogEntity = new AuditLogEntity()
            {
                Id = Guid.NewGuid(),
                Pk2name = auditInfo.Pk2name,
                Pk2value = auditInfo.Pk2value,
                Col2name = auditInfo.Col2name,
                Col2value = auditInfo.Col2value,
                Col3name = auditInfo.Col3name,
                Col3value = auditInfo.Col3value,
                DoneBy = entityEntry.Property(p => p.ChangedBy).CurrentValue,
                EventDate = entityEntry.Property(p => p.ChangedAt).CurrentValue ?? DateTime.UtcNow,
                EventTable = auditInfo.EventTable,
                EventType = entityEntry.State.ToAuditLogEventType(),
                EventDescription = string.Join(", ", changedProperties),
            };

            return auditLogEntity;
        }

        private AuditLogEntity CreateAuditLogEntityForDeletedEntry(EntityEntry<ParticipantEntity> entityEntry)
        {
            var auditInfo = GetAuditInfo(entityEntry);
            var auditLogEntity = new AuditLogEntity()
            {
                Id = Guid.NewGuid(),
                Pk2name = auditInfo.Pk2name,
                Pk2value = auditInfo.Pk2value,
                Col2name = auditInfo.Col2name,
                Col2value = auditInfo.Col2value,
                Col3name = auditInfo.Col3name,
                Col3value = auditInfo.Col3value,
                DoneBy = userContextProvider.CurrentUser.UserId,
                EventDate = entityEntry.Property(p => p.ChangedAt).CurrentValue ?? DateTime.UtcNow,
                EventTable = auditInfo.EventTable,
                EventType = entityEntry.State.ToAuditLogEventType(),
                EventDescription = entityEntry.State.ToString()
            };

            return auditLogEntity;
        }

        protected static ParticipantAuditLog GetAuditInfo(EntityEntry<ParticipantEntity> entityEntry)
        {
            return new ParticipantAuditLog(
                entityEntry.Entity.Id,
                entityEntry.Entity.FirstName!,
                entityEntry.Entity.LastName!,
                entityEntry.Entity.Birthdate);
        }
    }
}

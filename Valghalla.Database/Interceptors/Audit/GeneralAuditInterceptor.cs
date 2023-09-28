using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Valghalla.Application;
using Valghalla.Application.User;
using Valghalla.Database.Entities.Tables;
using Valghalla.Database.Interceptors.ChangeTracking;

namespace Valghalla.Database.Interceptors.Audit
{
    record AuditInfo(string TableName, Guid PkValue);

    internal class GeneralAuditInterceptor : SaveChangesInterceptor
    {
        private readonly IUserContextProvider userContextProvider;

        public GeneralAuditInterceptor(IUserContextProvider userContextProvider)
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

            // Participant entity will be tracked seperately
            var trackingEntries = dbContext.ChangeTracker
                .Entries<IChangeTrackingEntity>()
                .Where(i => i.Entity is not ParticipantEntity);

            var addedEntries = trackingEntries.Where(x => x.State == EntityState.Added);
            var modifiedEntries = trackingEntries.Where(x => x.State == EntityState.Modified);
            var deletedEntries = trackingEntries.Where(x => x.State == EntityState.Deleted);

            var auditLogsForAddedEntries = addedEntries.Select(i => CreateAuditLogEntityForAddedEntry(dbContext, i)).Where(i => i != null);
            var auditLogsForModifiedEntries = modifiedEntries.Select(i => CreateAuditLogEntityForModifiedEntry(dbContext, i)).Where(i => i != null);
            var auditLogsForDeletedEntries = deletedEntries.Select(i => CreateAuditLogEntityForDeletedEntry(dbContext, i)).Where(i => i != null);

            var auditLogEntities = new List<AuditLogEntity>();
            auditLogEntities.AddRange(auditLogsForAddedEntries!);
            auditLogEntities.AddRange(auditLogsForModifiedEntries!);
            auditLogEntities.AddRange(auditLogsForDeletedEntries!);

            if (!auditLogEntities.Any())
            {
                await Task.CompletedTask;
                return;
            }

            await dbContext.AddRangeAsync(auditLogEntities, cancellationToken);
        }

        private static AuditLogEntity? CreateAuditLogEntityForAddedEntry(DbContext dbContext, EntityEntry<IChangeTrackingEntity> entityEntry)
        {
            var auditInfo = GetAuditInfo(dbContext, entityEntry);

            if (auditInfo == null) return null;

            var auditLogEntity = new AuditLogEntity()
            {
                Id = Guid.NewGuid(),
                Pk2name = Constants.AuditLog.PrimaryKeyColumn,
                Pk2value = auditInfo.PkValue,
                DoneBy = entityEntry.Property(p => p.CreatedBy).CurrentValue,
                EventDate = entityEntry.Property(p => p.CreatedAt).CurrentValue,
                EventTable = auditInfo.TableName,
                EventType = entityEntry.State.ToAuditLogEventType(),
                EventDescription = entityEntry.State.ToString(),
            };

            return auditLogEntity;
        }

        private static AuditLogEntity? CreateAuditLogEntityForModifiedEntry(DbContext dbContext, EntityEntry<IChangeTrackingEntity> entityEntry)
        {
            var auditInfo = GetAuditInfo(dbContext, entityEntry);

            if (auditInfo == null) return null;

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
                Pk2name = Constants.AuditLog.PrimaryKeyColumn,
                Pk2value = auditInfo.PkValue,
                DoneBy = entityEntry.Property(p => p.ChangedBy).CurrentValue,
                EventDate = entityEntry.Property(p => p.ChangedAt).CurrentValue ?? DateTime.UtcNow,
                EventTable = auditInfo.TableName,
                EventType = entityEntry.State.ToAuditLogEventType(),
                EventDescription = string.Join(", ", changedProperties),
            };

            return auditLogEntity;
        }

        private AuditLogEntity? CreateAuditLogEntityForDeletedEntry(DbContext dbContext, EntityEntry<IChangeTrackingEntity> entityEntry)
        {
            var auditInfo = GetAuditInfo(dbContext, entityEntry);

            if (auditInfo == null) return null;

            var auditLogEntity = new AuditLogEntity()
            {
                Id = Guid.NewGuid(),
                Pk2name = Constants.AuditLog.PrimaryKeyColumn,
                Pk2value = auditInfo.PkValue,
                DoneBy = userContextProvider.CurrentUser.UserId,
                EventDate = entityEntry.Property(p => p.ChangedAt).CurrentValue ?? DateTime.UtcNow,
                EventTable = auditInfo.TableName,
                EventType = entityEntry.State.ToAuditLogEventType(),
                EventDescription = entityEntry.State.ToString()
            };

            return auditLogEntity;
        }

        protected static AuditInfo? GetAuditInfo(DbContext dbContext, EntityEntry<IChangeTrackingEntity> entityEntry)
        {
            var clrType = entityEntry.Entity.GetType();

            var entityType = dbContext.Model.FindEntityType(clrType)
                ?? throw new Exception("Could not find entity type with CLR type: " + clrType.FullName);

            var tableName = entityType.GetSchemaQualifiedTableName()
                ?? throw new Exception("Could not find table name with CLR type: " + clrType.FullName);

            var pkPropertyEntry = entityEntry.Properties.FirstOrDefault(prop => prop.Metadata.PropertyInfo != null && prop.Metadata.PropertyInfo.Name == Constants.AuditLog.PrimaryKeyColumn);

            if (pkPropertyEntry == null) return null;

            if (pkPropertyEntry.CurrentValue == null)
            {
                throw new Exception("Primary key value is null");
            }

            return new AuditInfo(tableName, Guid.Parse(pkPropertyEntry.CurrentValue + string.Empty));
        }
    }
}

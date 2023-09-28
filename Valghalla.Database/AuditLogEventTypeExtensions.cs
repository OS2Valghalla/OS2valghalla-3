using Microsoft.EntityFrameworkCore;
using Valghalla.Application.AuditLog;

namespace Valghalla.Database
{
    public static class AuditLogEventTypeExtensions
    {
        public static string ToAuditLogEventType(this EntityState entityState)
        {
            return entityState switch
            {
                EntityState.Added => AuditLogEventType.Create.Value,
                EntityState.Modified => AuditLogEventType.Edit.Value,
                EntityState.Deleted => AuditLogEventType.Delete.Value,
                _ => entityState.ToString(),
            };
        }
    }
}

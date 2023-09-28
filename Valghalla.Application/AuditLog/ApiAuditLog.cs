namespace Valghalla.Application.AuditLog
{
    public sealed record ApiAuditLog : AuditLogBase
    {
        public override string EventTable => AuditLogEventTable.API.Value;
        public override string EventType => AuditLogEventType.Request.Value;

        public ApiAuditLog(string path)
        {
            EventDescription = path;
        }
    }
}

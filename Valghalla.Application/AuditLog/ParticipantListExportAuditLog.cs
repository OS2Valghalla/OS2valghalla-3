namespace Valghalla.Application.AuditLog
{
    public sealed record ParticipantListExportAuditLog : AuditLogBase
    {
        public override string EventTable => AuditLogEventTable.List.Value;
        public override string EventType => AuditLogEventType.Export.Value;
    }
}

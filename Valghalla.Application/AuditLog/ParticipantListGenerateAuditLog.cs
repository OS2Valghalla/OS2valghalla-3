namespace Valghalla.Application.AuditLog
{
    public sealed record ParticipantListGenerateAuditLog : AuditLogBase
    {
        public override string EventTable => AuditLogEventTable.List.Value;
        public override string EventType => AuditLogEventType.Generate.Value;
    }
}

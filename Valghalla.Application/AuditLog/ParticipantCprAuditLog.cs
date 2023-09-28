namespace Valghalla.Application.AuditLog
{
    public sealed record ParticipantCprAuditLog : AuditLogBase
    {
        public override string EventTable => AuditLogEventTable.Participant.Value;
        public override string EventType => AuditLogEventType.LookUpCpr.Value;

        public ParticipantCprAuditLog(string cpr)
        {
            EventDescription = cpr;
        }
    }
}

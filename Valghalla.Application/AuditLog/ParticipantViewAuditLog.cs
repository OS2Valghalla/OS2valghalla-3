namespace Valghalla.Application.AuditLog
{
    public sealed record ParticipantViewAuditLog : ParticipantAuditLog
    {
        public override string EventType => AuditLogEventType.View.Value;

        public ParticipantViewAuditLog(Guid participantId, string firstName, string lastName, DateTime birthDate) : base(participantId, firstName, lastName, birthDate)
        {

        }
    }
}

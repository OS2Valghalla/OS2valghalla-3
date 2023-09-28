namespace Valghalla.Application.AuditLog
{
    public sealed record ParticipantTeamResponsibleAuditLog : ParticipantAuditLog
    {
        public override string EventTable => AuditLogEventTable.TeamResponsible.Value;
        public override string EventType => AuditLogEventType.Edit.Value;

        public ParticipantTeamResponsibleAuditLog(bool added, string teamName, Guid participantId, string firstName, string lastName, DateTime birthDate) : base(participantId, firstName, lastName, birthDate)
        {
            EventDescription = $"Team: {teamName}" + " - " + "Permission: " + (added ? "Added" : "Removed");
        }
    }
}

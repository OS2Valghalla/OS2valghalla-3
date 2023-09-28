namespace Valghalla.Application.AuditLog
{
    public sealed record ParticipantWorkLocationResponsibleAuditLog : ParticipantAuditLog
    {
        public override string EventTable => AuditLogEventTable.WorkLocationResponsible.Value;
        public override string EventType => AuditLogEventType.Edit.Value;

        public ParticipantWorkLocationResponsibleAuditLog(bool added, string workLocationName, Guid participantId, string firstName, string lastName, DateTime birthDate) : base(participantId, firstName, lastName, birthDate)
        {
            EventDescription = $"WorkLocation: {workLocationName}" + " - " + "Permission: " + (added ? "Added" : "Removed");
        }
    }
}

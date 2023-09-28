namespace Valghalla.Application.AuditLog
{
    public record ParticipantAuditLog : AuditLogBase
    {
        public override string EventTable => AuditLogEventTable.Participant.Value;
        public override string Pk2name => Constants.AuditLog.PrimaryKeyColumn;
        public override string Col2name => Constants.AuditLog.ParticipantNameColumn;
        public override string Col3name => Constants.AuditLog.ParticipantBirthdateColumn;

        public ParticipantAuditLog(Guid participantId, string firstName, string lastName, DateTime birthDate)
        {
            Pk2value = participantId;
            Col2value = firstName + " " + lastName;
            Col3value = $"{PadTimeValue(birthDate.Day)}/{PadTimeValue(birthDate.Month)}/{PadTimeValue(birthDate.Year)}";
        }

        private static string PadTimeValue(int value) => value.ToString().PadLeft(2, '0');
    }
}

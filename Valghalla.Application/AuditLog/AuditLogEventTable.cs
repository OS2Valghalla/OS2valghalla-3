namespace Valghalla.Application.AuditLog
{
    public class AuditLogEventTable
    {
        public static readonly AuditLogEventTable Participant = new("Participant", "administration.audit_log.event_table.participant");
        public static readonly AuditLogEventTable List = new("List", "administration.audit_log.event_table.list");
        public static readonly AuditLogEventTable TeamResponsible = new("TeamResponsible", "administration.audit_log.event_table.team_responsible");
        public static readonly AuditLogEventTable WorkLocationResponsible = new("WorkLocationResponsible", "administration.audit_log.event_table.work_location_responsible");
        public static readonly AuditLogEventTable API = new("API", "administration.audit_log.event_table.api");
        public static readonly AuditLogEventTable Others = new("Others", "administration.audit_log.event_table.others");

        public string Value { get; init; }
        public string Name { get; init; }

        private AuditLogEventTable(string value, string name)
        {
            Value = value;
            Name = name;
        }

        public static IEnumerable<AuditLogEventTable> GetAll() => new AuditLogEventTable[]
        {
            Participant,
            List,
            TeamResponsible,
            WorkLocationResponsible,
            API,
            Others,
        };
    }
}

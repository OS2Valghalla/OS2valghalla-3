namespace Valghalla.Application.AuditLog
{
    public class AuditLogEventType
    {
        public static readonly AuditLogEventType Create = new("Create", "administration.audit_log.event_type.create");
        public static readonly AuditLogEventType View = new("View", "administration.audit_log.event_type.view");
        public static readonly AuditLogEventType Edit = new("Edit", "administration.audit_log.event_type.edit");
        public static readonly AuditLogEventType Delete = new("Delete", "administration.audit_log.event_type.delete");
        public static readonly AuditLogEventType LookUpCpr = new("LookUpCpr", "administration.audit_log.event_type.look_up_cpr");
        public static readonly AuditLogEventType Generate = new("Generate", "administration.audit_log.event_type.generate");
        public static readonly AuditLogEventType Export = new("Export", "administration.audit_log.event_type.export");
        public static readonly AuditLogEventType Request = new("Request", "administration.audit_log.event_type.request");

        public string Value { get; init; }
        public string Name { get; init; }

        private AuditLogEventType(string value, string name)
        {
            Value = value;
            Name = name;
        }

        public static IEnumerable<AuditLogEventType> GetAll() => new AuditLogEventType[]
        {
            Create,
            View,
            Edit,
            Delete,
            LookUpCpr,
            Generate,
            Export,
            Request,
        };
    }
}

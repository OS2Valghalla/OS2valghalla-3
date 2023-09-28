namespace Valghalla.Application.AuditLog
{
    public abstract record AuditLogBase
    {
        public virtual string? Pk2name { get; init; }
        public virtual Guid? Pk2value { get; init; }
        public virtual string? Col2name { get; init; }
        public virtual string? Col2value { get; init; }
        public virtual string? Col3name { get; init; }
        public virtual string? Col3value { get; init; }
        public virtual string EventTable { get; init; } = null!;
        public virtual string EventType { get; init; } = null!;
        public virtual string? EventDescription { get; init; }
    }
}

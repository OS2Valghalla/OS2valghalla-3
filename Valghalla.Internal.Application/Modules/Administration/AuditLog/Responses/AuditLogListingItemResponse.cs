namespace Valghalla.Internal.Application.Modules.Administration.AuditLog.Responses
{
    public sealed record AuditLogListingItemResponse
    {
        public Guid Id { get; init; }
        public string? Pk2name { get; init; }
        public Guid? Pk2value { get; init; }
        public string? Col2name { get; init; }
        public string? Col2value { get; init; }
        public string? Col3name { get; init; }
        public string? Col3value { get; init; }
        public string EventTable { get; init; } = null!;
        public string EventType { get; init; } = null!;
        public string? EventDescription { get; init; }
        public string? DoneBy { get; init; }
        public DateTime EventDate { get; init; }
    }
}

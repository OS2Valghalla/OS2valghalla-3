namespace Valghalla.Internal.Application.Modules.Tasks.Requests
{
    public sealed record ReplyForParticipantRequest
    {
        public Guid TaskAssignmentId { get; set; }
        public Guid ElectionId { get; set; }
        public bool Accepted { get; set; }
        public string? MobileNumber { get; init; }
        public string? Email { get; init; }
        public IEnumerable<Guid> SpecialDietIds { get; init; } = Enumerable.Empty<Guid>();
    }
}

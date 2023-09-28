namespace Valghalla.Internal.API.Requests.Administration.Communication
{
    public sealed record SendGroupMessageRequest
    {
        public Guid ElectionId { get; init; }
        public string Subject { get; init; } = string.Empty;
        public string Content { get; init; } = string.Empty;
        public int TemplateType { get; init; }
        public IEnumerable<Guid> FileReferenceIds { get; init; } = Enumerable.Empty<Guid>();
        public IEnumerable<Guid> TeamIds { get; init; } = Enumerable.Empty<Guid>();
        public IEnumerable<Guid> WorkLocationIds { get; init; } = Enumerable.Empty<Guid>();
        public IEnumerable<Guid> TaskTypeIds { get; init; } = Enumerable.Empty<Guid>();
        public IEnumerable<DateTime> TaskDates { get; init; } = Enumerable.Empty<DateTime>();
        public IEnumerable<Valghalla.Application.Enums.TaskStatus> TaskStatuses { get; init; } = Enumerable.Empty<Valghalla.Application.Enums.TaskStatus>();
    }
}

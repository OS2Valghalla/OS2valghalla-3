namespace Valghalla.Internal.API.Requests.Administration.TaskType
{
    public sealed record CreateTaskTypeRequest
    {
        public string Title { get; init; } = null!;
        public string ShortName { get; init; } = null!;
        public string Description { get; init; } = null!;
        public TimeSpan StartTime { get; init; }
        public TimeSpan EndTime { get; init; }
        public int? Payment { get; init; }
        public bool ValidationNotRequired { get; init; }
        public bool Trusted { get; init; }
        public bool SendingReminderEnabled { get; init; } = true;
        public IEnumerable<Guid> FileReferenceIds { get; init; } = Enumerable.Empty<Guid>();
    }
}

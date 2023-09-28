namespace Valghalla.External.Application.Modules.Tasks.Responses
{
    public sealed record TaskAssignmentResponse
    {
        public Guid Id { get; init; }
        public Guid ElectionId { get; init; }
        public Guid TaskTypeId { get; init; }
        public DateTime TaskDate { get; init; }
        public TimeSpan StartTime { get; init; }
        public TimeSpan EndTime { get; init; }
    }
}

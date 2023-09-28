namespace Valghalla.External.Application.Modules.WorkLocation.Responses
{
    public sealed record WorkLocationTaskTypeDetailsResponse
    {
        public Guid Id { get; init; }
        public string Title { get; init; } = null!;
        public TimeSpan StartTime { get; init; }
        public TimeSpan EndTime { get; init; }
        public int AcceptedTasksCount { get; init; }
        public int AllTasksCount { get; init; }
    }
}

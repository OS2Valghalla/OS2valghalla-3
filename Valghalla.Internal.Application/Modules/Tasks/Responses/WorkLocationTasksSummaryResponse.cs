namespace Valghalla.Internal.Application.Modules.Tasks.Responses
{
    public sealed record WorkLocationTasksSummaryResponse
    {
        public Guid WorkLocationId { get; init; }
        public Guid TaskTypeId { get; init; }
        public int AssignedTasksCount { get; init; }
        public int MissingTasksCount { get; init; }
        public int AwaitingTasksCount { get; init; }
        public int AllTasksCount { get; init; }
    }
}

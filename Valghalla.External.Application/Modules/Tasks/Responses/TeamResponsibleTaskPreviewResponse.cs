using Valghalla.Application.Storage;

namespace Valghalla.External.Application.Modules.Tasks.Responses
{
    public sealed record TeamResponsibleTaskPreviewResponse
    {
        public string HashValue { get; init; } = null!;
        public DateTime TaskDate { get; init; }
        public TaskPreviewTeam Team { get; init; } = null!;
        public TeamResponsibleTaskPreviewTaskType TaskType { get; init; } = null!;
        public TaskPreviewWorkLocation WorkLocation { get; init; } = null!;
    }

    public sealed record TeamResponsibleTaskPreviewTaskType
    {
        public Guid Id { get; init; }
        public string Title { get; init; } = null!;
        public string Description { get; init; } = null!;
        public TimeSpan StartTime { get; init; }
        public TimeSpan EndTime { get; init; }
        public int? Payment { get; init; }
        public bool Trusted { get; set; }
        public int AcceptedTasksCount { get; set; }
        public int UnansweredTasksCount { get; set; }
        public int AllTasksCount { get; set; }
    }
}

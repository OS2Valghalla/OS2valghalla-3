namespace Valghalla.External.Application.Modules.Tasks.Responses
{
    public sealed record TeamResponsibleTasksFiltersOptionsResponse
    {
        public IList<TaskPreviewTeam> Teams { get; set; } = new List<TaskPreviewTeam>();
        public IList<TaskPreviewTaskType> TaskTypes { get; set; } = new List<TaskPreviewTaskType>();
        public IList<TaskPreviewWorkLocation> WorkLocations { get; set; } = new List<TaskPreviewWorkLocation>();
    }
}

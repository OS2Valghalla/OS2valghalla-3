namespace Valghalla.External.Application.Modules.Team.Responses
{
    public sealed record TeamMemberResponse
    {
        public Guid Id { get; init; }
        public string Name { get; init; } = null!;
        public int AssignedTasksCount { get; set; }
        public bool CanBeRemoved { get; set; }
        public IList<TeamMemberWorkLocationDetailsResponse> WorkLocations { get; set; } = new List<TeamMemberWorkLocationDetailsResponse>();
    }

    public sealed record TeamMemberWorkLocationDetailsResponse
    {
        public string WorkLocationTitle { get; set; } = string.Empty;
        public IList<TeamMemberTaskDetailsResponse> Tasks { get; set; } = new List<TeamMemberTaskDetailsResponse>();
    }

    public sealed record TeamMemberTaskDetailsResponse
    {
        public string TaskTitle { get; set; } = string.Empty;
        public int TaskStatus { get; set; }
        public DateOnly TaskDate { get; set; }
    }
}

namespace Valghalla.External.Application.Modules.Team.Responses
{
    public sealed record TeamMemberResponse
    {
        public Guid Id { get; init; }
        public string Name { get; init; } = null!;
        public int AssignedTasksCount { get; set; }
        public bool CanBeRemoved { get; set; }
    }
}

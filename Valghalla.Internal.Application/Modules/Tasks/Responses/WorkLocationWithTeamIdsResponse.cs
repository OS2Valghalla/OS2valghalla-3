namespace Valghalla.Internal.Application.Modules.Tasks.Responses
{
    public sealed record WorkLocationWithTeamIdsResponse
    {
        public Guid Id { get; init; }
        public string Title { get; init; } = null!;
        public List<Guid> TeamIds { get; set; } = new List<Guid>();
    }
}

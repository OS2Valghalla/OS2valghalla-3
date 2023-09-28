namespace Valghalla.Internal.Application.Modules.Tasks.Responses
{
    public sealed record TaskTypeWithTeamIdsResponse
    {
        public Guid Id { get; init; }
        public string Title { get; init; } = null!;
        public string ShortName { get; set; } = null!;
        public List<Guid> TeamIds { get; set; } = new List<Guid>();
    }
}

namespace Valghalla.Internal.Application.Modules.Administration.Team.Responses
{
    public sealed record TeamDetailResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string? ShortName { get; set; }
        public string? Description { get; set; }
        public int TaskCount { get; set; }
        public IEnumerable<Guid> ResponsibleIds { get; init; } = Enumerable.Empty<Guid>(); 
    }
}

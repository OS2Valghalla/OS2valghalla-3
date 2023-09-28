namespace Valghalla.Internal.Application.Modules.Administration.Team.Responses
{
    public record ListTeamsItemResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string ShortName { get; set; } = null!;
        public string? Description { get; set; }
        public int ResponsibleCount { get; set; }
        public int TaskCount { get; set; }
    }
}

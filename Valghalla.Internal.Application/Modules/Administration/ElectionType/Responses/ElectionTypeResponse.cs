namespace Valghalla.Internal.Application.Modules.Administration.ElectionType.Responses
{
    public record ElectionTypeResponse
    {
        public Guid Id { get; init; }
        public List<Guid> ValidationRuleIds { get; init; } = new List<Guid>();
        public string Title { get; init; } = null!;
    }
}

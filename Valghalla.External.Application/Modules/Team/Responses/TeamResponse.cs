namespace Valghalla.External.Application.Modules.Team.Responses
{
    public sealed record TeamResponse
    {
        public Guid Id { get; init; }
        public string Name { get; init; } = null!;
    }
}

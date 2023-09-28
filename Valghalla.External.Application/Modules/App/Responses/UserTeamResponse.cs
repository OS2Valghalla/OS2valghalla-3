namespace Valghalla.External.Application.Modules.App.Responses
{
    public sealed record UserTeamResponse
    {
        public Guid Id { get; init; }
        public string Name { get; init; } = null!;
    }
}

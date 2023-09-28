namespace Valghalla.Internal.Application.Modules.Administration.Area.Responses
{
    public sealed record AreaDetailsResponse
    {
        public Guid Id { get; init; }
        public string Name { get; init; } = null!;
        public string? Description { get; init; }
    }
}

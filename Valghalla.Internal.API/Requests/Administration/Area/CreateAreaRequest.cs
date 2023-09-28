namespace Valghalla.Internal.API.Requests.Administration.Area
{
    public sealed record CreateAreaRequest
    {
        public string Name { get; init; } = string.Empty;
        public string? Description { get; init; }
    }
}
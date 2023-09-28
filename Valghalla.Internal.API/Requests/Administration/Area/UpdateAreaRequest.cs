namespace Valghalla.Internal.API.Requests.Administration.Area
{
    public sealed record UpdateAreaRequest
    {
        public Guid Id { get; init; }
        public string Name { get; init; } = string.Empty;
        public string? Description { get; init; }
    }
}
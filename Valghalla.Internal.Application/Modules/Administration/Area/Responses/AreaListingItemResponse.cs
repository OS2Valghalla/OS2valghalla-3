namespace Valghalla.Internal.Application.Modules.Administration.Area.Responses
{
    public sealed record AreaListingItemResponse
    {
        public Guid Id { get; init; }
        public string Name { get; init; } = null!;
    }
}

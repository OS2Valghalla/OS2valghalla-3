namespace Valghalla.Internal.Application.Modules.Administration.Election.Responses
{
    public sealed record ElectionListingItemResponse
    {
        public Guid Id { get; init; }
        public string Title { get; init; } = null!;
        public DateTime ElectionDate { get; init; }
        public bool Active { get; init; }
        public Guid ElectionTypeId { get; init; }
        public string ElectionTypeName { get; init; } = null!;
    }
}

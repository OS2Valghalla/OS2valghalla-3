namespace Valghalla.Internal.Application.Modules.Shared.WorkLocation.Responses
{
    public sealed record WorkLocationSharedResponse
    {
        public Guid Id { get; init; }
        public Guid AreaId { get; set; }
        public string Title { get; init; } = null!;
    }
}

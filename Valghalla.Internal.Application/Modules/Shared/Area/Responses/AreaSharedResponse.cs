namespace Valghalla.Internal.Application.Modules.Shared.Area.Responses
{
    public sealed record AreaSharedResponse
    {
        public Guid Id { get; init; }
        public string Name { get; set; } = null!;
    }
}

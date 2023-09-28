namespace Valghalla.External.Application.Modules.Shared.WorkLocation.Responses
{
    public sealed record WorkLocationSharedResponse
    {
        public Guid Id { get; init; }
        public string Title { get; init; } = null!;
    }
}

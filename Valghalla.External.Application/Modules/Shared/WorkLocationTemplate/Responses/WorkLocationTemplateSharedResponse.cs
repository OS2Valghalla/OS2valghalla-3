namespace Valghalla.External.Application.Modules.Shared.WorkLocationTemplate.Responses
{
    public sealed record WorkLocationTemplateSharedResponse
    {
        public Guid Id { get; init; }
        public string Title { get; init; } = null!;
    }
}

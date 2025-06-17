namespace Valghalla.Internal.Application.Modules.Shared.WorkLocationTemplate.Responses
{
    public sealed record WorkLocationTemplateSharedResponse
    {
        public Guid Id { get; init; }
        public Guid AreaId { get; set; }
        public string Title { get; init; } = null!;
    }
}

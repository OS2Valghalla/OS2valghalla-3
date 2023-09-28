namespace Valghalla.Application.Communication
{
    public sealed record CommunicationTemplate
    {
        public string Subject { get; init; } = null!;
        public string Content { get; init; } = null!;
        public int TemplateType { get; init; }
        public IEnumerable<Guid> FileRefIds { get; init; } = Enumerable.Empty<Guid>();
    }
}

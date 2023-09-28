namespace Valghalla.Internal.API.Requests.Administration.Communication
{
    public sealed record UpdateCommunicationTemplateRequest
    {
        public Guid Id { get; init; }
        public string Title { get; init; } = null!;
        public string? Subject { get; set; }
        public string? Content { get; set; }
        public int TemplateType { get; set; }
        public IEnumerable<Guid> FileReferenceIds { get; init; } = Enumerable.Empty<Guid>();
    }
}

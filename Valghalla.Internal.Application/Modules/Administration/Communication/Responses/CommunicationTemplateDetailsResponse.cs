using Valghalla.Application.Storage;

namespace Valghalla.Internal.Application.Modules.Administration.Communication.Responses
{
    public sealed record CommunicationTemplateDetailsResponse
    {
        public Guid Id { get; init; }
        public string Title { get; set; } = null!;
        public string? Subject { get; set; }
        public string? Content { get; set; }
        public int TemplateType { get; set; }
        public bool? IsDefaultTemplate { get; set; }
        public IEnumerable<FileReferenceInfo> CommunicationTemplateFileReferences { get; init; } = Enumerable.Empty<FileReferenceInfo>();

    }
}

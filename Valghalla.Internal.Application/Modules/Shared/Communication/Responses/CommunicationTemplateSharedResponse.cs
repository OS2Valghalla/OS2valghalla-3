namespace Valghalla.Internal.Application.Modules.Shared.Communication.Responses
{
    public sealed record CommunicationTemplateSharedResponse
    {
        public Guid Id { get; init; }
        public string Title { get; init; } = null!;
        public int TemplateType { get; set; }
        public bool? IsDefaultTemplate { get; set; }
    }
}

namespace Valghalla.Application.Queue.Messages
{
    public sealed record SendGroupMessageJobMessage
    {
        public Guid ParticipantId { get; init; }
        public Guid TaskId { get; init; }
        public bool IsRejectedTask { get; init; }
        public int TemplateType { get; init; }
        public string TemplateSubject { get; init; } = string.Empty;
        public string TemplateContent { get; init; } = string.Empty;
        public IList<Guid> TemplateFileReferenceIds { get; init; }

    }
}

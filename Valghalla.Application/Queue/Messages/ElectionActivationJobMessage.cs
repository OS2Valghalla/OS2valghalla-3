namespace Valghalla.Application.Queue.Messages
{
    public sealed record ElectionActivationJobMessage
    {
        public Guid ElectionId { get; init; }
    }
}

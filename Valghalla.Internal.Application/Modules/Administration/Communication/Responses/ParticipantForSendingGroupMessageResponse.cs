namespace Valghalla.Internal.Application.Modules.Administration.Communication.Responses
{
    public sealed record ParticipantForSendingGroupMessageResponse
    {
        public Guid? ParticipantId { get; set; }
        public string? ParticipantName { get; set; }
        public IList<Guid> TeamIds { get; set; } = new List<Guid>();
    }
}

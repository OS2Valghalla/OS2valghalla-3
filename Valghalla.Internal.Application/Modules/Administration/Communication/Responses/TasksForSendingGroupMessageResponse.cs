namespace Valghalla.Internal.Application.Modules.Administration.Communication.Responses
{
    public sealed record TasksForSendingGroupMessageResponse
    {
        public IList<Guid> AssignedTaskIds { get; set; } = new List<Guid>();
        public IList<Guid> RejectedTaskIds { get; set; } = new List<Guid>();
    }
}

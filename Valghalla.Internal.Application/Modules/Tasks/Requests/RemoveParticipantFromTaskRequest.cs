namespace Valghalla.Internal.Application.Modules.Tasks.Requests
{
    public sealed record RemoveParticipantFromTaskRequest
    {
        public Guid ElectionId { get; set; }
        public Guid TaskAssignmentId { get; set; }
    }
}

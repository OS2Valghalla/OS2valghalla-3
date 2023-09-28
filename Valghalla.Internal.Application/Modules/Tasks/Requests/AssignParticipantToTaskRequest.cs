namespace Valghalla.Internal.Application.Modules.Tasks.Requests
{
    public sealed record AssignParticipantToTaskRequest
    {
        public Guid ElectionId { get; set; }
        public Guid TaskAssignmentId { get; set; }
        public Guid ParticipantId { get; set; }
        public Guid TaskTypeId { get; set; }
    }
}

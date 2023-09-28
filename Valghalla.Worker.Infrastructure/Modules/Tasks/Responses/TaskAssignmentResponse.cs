namespace Valghalla.Worker.Infrastructure.Modules.Tasks.Responses
{
    public sealed record TaskAssignmentResponse
    {
        public Guid Id { get; init; }
        public Guid ParticipantId { get; init; }
        public Guid TaskTypeId { get; init; }
        public Guid ElectionId { get; init; }
    }
}

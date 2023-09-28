using Valghalla.Internal.Application.Modules.Shared.Participant.Responses;

namespace Valghalla.Internal.Application.Modules.Tasks.Responses
{
    public sealed record TaskAssignmentResponse
    {
        public Guid Id { get; set; }
        public Guid ElectionId { get; set; }
        public Guid WorkLocationId { get; set; }
        public string? WorkLocationName { get; set; }
        public Guid TaskTypeId { get; set; }
        public string? TaskTypeName { get; set; }
        public Guid TeamId { get; set; }
        public string? TeamName { get; set; }
        public Guid? ParticipantId { get; set; }
        public string? ParticipantName { get; set; }
        public DateTime TaskDate { get; set; }
        public bool Responsed { get; set; }
        public bool Accepted { get; set; }
        public string? TaskDetailsPageUrl { get; set; }
    }
}

using Valghalla.Internal.Application.Modules.Administration.Election.Requests;

namespace Valghalla.Internal.API.Requests.Administration.Election
{
    public sealed record DuplicateElectionRequest
    {
        public Guid SourceElectionId { get; init; }
        public string Title { get; init; } = string.Empty;
        public Guid ElectionTypeId { get; init; }
        public int LockPeriod { get; init; }
        public DateTime ElectionDate { get; init; }
        public int DaysBeforeElectionDate { get; init; }
        public int DaysAfterElectionDate { get; init; }
        public Guid? ConfirmationOfRegistrationCommunicationTemplateId { get; set; }
        public Guid? ConfirmationOfCancellationCommunicationTemplateId { get; set; }
        public Guid? InvitationCommunicationTemplateId { get; set; }
        public Guid? InvitationReminderCommunicationTemplateId { get; set; }
        public Guid? TaskReminderCommunicationTemplateId { get; set; }
        public Guid? RetractedInvitationCommunicationTemplateId { get; set; }
        public Guid? RemovedFromTaskCommunicationTemplateId { get; set; }
        public Guid? RemovedByValidationCommunicationTemplateId { get; set; }
        public IEnumerable<Guid> WorkLocationIds { get; init; } = Array.Empty<Guid>();
        public IEnumerable<CreateElectionTaskTypeCommunicationTemplateRequest> ElectionTaskTypeCommunicationTemplates { get; init; } = Array.Empty<CreateElectionTaskTypeCommunicationTemplateRequest>();
    }
}

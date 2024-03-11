using Valghalla.Internal.Application.Modules.Administration.Election.Requests;

namespace Valghalla.Internal.API.Requests.Administration.Election
{
    public sealed record UpdateElectionCommunicationConfigurationsRequest
    {
        public Guid Id { get; init; }
        public Guid? ConfirmationOfRegistrationCommunicationTemplateId { get; set; }
        public Guid? ConfirmationOfCancellationCommunicationTemplateId { get; set; }
        public Guid? InvitationCommunicationTemplateId { get; set; }
        public Guid? InvitationReminderCommunicationTemplateId { get; set; }
        public Guid? TaskReminderCommunicationTemplateId { get; set; }
        public Guid? RetractedInvitationCommunicationTemplateId { get; set; }
        public Guid? RemovedFromTaskCommunicationTemplateId { get; set; }
        public Guid? RemovedByValidationCommunicationTemplateId { get; set; }
        public IList<UpdateElectionTaskTypeCommunicationTemplateRequest> ElectionTaskTypeCommunicationTemplates { get; set; } = new List<UpdateElectionTaskTypeCommunicationTemplateRequest>();
    }
}

namespace Valghalla.Internal.Application.Modules.Administration.Election.Requests
{
    public sealed record CreateElectionTaskTypeCommunicationTemplateRequest
    {
        public Guid TaskTypeId { get; set; }
        public Guid? ConfirmationOfRegistrationCommunicationTemplateId { get; set; }
        public Guid? ConfirmationOfCancellationCommunicationTemplateId { get; set; }
        public Guid? InvitationCommunicationTemplateId { get; set; }
        public Guid? InvitationReminderCommunicationTemplateId { get; set; }
        public Guid? TaskReminderCommunicationTemplateId { get; set; }
        public Guid? RetractedInvitationCommunicationTemplateId { get; set; }
        public Guid? RemovedFromTaskCommunicationTemplateId { get; set; }
        public Guid? RemovedByValidationCommunicationTemplateId { get; set; }
    }
}

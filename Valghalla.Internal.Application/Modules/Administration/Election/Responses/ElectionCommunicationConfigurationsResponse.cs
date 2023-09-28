using Valghalla.Internal.Application.Modules.Administration.Communication.Responses;
using Valghalla.Internal.Application.Modules.Administration.Election.Requests;

namespace Valghalla.Internal.Application.Modules.Administration.Election.Responses
{
    public sealed record ElectionCommunicationConfigurationsResponse
    {
        public Guid Id { get; init; }
        public string Title { get; init; } = null!;
        public CommunicationTemplateListingItemResponse? ConfirmationOfRegistrationCommunicationTemplate { get; set; }
        public CommunicationTemplateListingItemResponse? ConfirmationOfCancellationCommunicationTemplate { get; set; }
        public CommunicationTemplateListingItemResponse? InvitationCommunicationTemplate { get; set; }
        public CommunicationTemplateListingItemResponse? InvitationReminderCommunicationTemplate { get; set; }
        public CommunicationTemplateListingItemResponse? TaskReminderCommunicationTemplate { get; set; }
        public CommunicationTemplateListingItemResponse? RetractedInvitationCommunicationTemplate { get; set; }
        public IList<ElectionTaskTypeCommunicationTemplateResponse> ElectionTaskTypeCommunicationTemplates { get; set; } = new List<ElectionTaskTypeCommunicationTemplateResponse>();

    }
}

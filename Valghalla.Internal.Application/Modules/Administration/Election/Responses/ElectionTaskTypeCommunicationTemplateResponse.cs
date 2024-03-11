﻿using Valghalla.Internal.Application.Modules.Administration.Communication.Responses;

namespace Valghalla.Internal.Application.Modules.Administration.Election.Responses
{
    public sealed record ElectionTaskTypeCommunicationTemplateResponse
    {
        public Guid ElectionId { get; set; }
        public Guid TaskTypeId { get; set; }
        public CommunicationTemplateListingItemResponse? ConfirmationOfRegistrationCommunicationTemplate { get; set; }
        public CommunicationTemplateListingItemResponse? ConfirmationOfCancellationCommunicationTemplate { get; set; }
        public CommunicationTemplateListingItemResponse? InvitationCommunicationTemplate { get; set; }
        public CommunicationTemplateListingItemResponse? InvitationReminderCommunicationTemplate { get; set; }
        public CommunicationTemplateListingItemResponse? TaskReminderCommunicationTemplate { get; set; }
        public CommunicationTemplateListingItemResponse? RetractedInvitationCommunicationTemplate { get; set; }
        public CommunicationTemplateListingItemResponse? RemovedFromTaskCommunicationTemplate { get; set; }
        public CommunicationTemplateListingItemResponse? RemovedByValidationCommunicationTemplate { get; set; }
    }
}

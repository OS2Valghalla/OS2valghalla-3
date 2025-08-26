import { CommunicationTemplateListingItem } from '../../../communication/communication-template/models/communication-template-listing-item';

export interface ElectionTaskTypeCommunicationTemplate {
  electionId: string;
  taskTypeId: string;
  confirmationOfRegistrationCommunicationTemplate?: CommunicationTemplateListingItem;
  confirmationOfCancellationCommunicationTemplate?: CommunicationTemplateListingItem;
  invitationCommunicationTemplate?: CommunicationTemplateListingItem;
  invitationReminderCommunicationTemplate?: CommunicationTemplateListingItem;
  taskReminderCommunicationTemplate?: CommunicationTemplateListingItem;
  retractedInvitationCommunicationTemplate?: CommunicationTemplateListingItem;
  removedFromTaskCommunicationTemplate?: CommunicationTemplateListingItem;
  removedByValidationCommunicationTemplate?: CommunicationTemplateListingItem;
}

export interface ElectionCommunicationConfigurations {
  id: string;
  title: string;
  confirmationOfRegistrationCommunicationTemplate?: CommunicationTemplateListingItem;
  confirmationOfCancellationCommunicationTemplate?: CommunicationTemplateListingItem;
  invitationCommunicationTemplate?: CommunicationTemplateListingItem;
  invitationReminderCommunicationTemplate?: CommunicationTemplateListingItem;
  taskReminderCommunicationTemplate?: CommunicationTemplateListingItem;
  retractedInvitationCommunicationTemplate?: CommunicationTemplateListingItem;
  removedFromTaskCommunicationTemplate?: CommunicationTemplateListingItem;
  removedByValidationCommunicationTemplate?: CommunicationTemplateListingItem;
  electionTaskTypeCommunicationTemplates?: Array<ElectionTaskTypeCommunicationTemplate>;
}
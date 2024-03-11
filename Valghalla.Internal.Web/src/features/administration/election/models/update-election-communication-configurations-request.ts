export interface UpdateElectionTaskTypeCommunicationTemplateRequest {
  taskTypeId: string;
  confirmationOfRegistrationCommunicationTemplateId?: string;
  confirmationOfCancellationCommunicationTemplateId?: string;
  invitationCommunicationTemplateId?: string;
  invitationReminderCommunicationTemplateId?: string;
  taskReminderCommunicationTemplateId?: string;
  retractedInvitationCommunicationTemplateId?: string;
  removedFromTaskCommunicationTemplateId?: string;
  removedByValidationCommunicationTemplateId?: string;
}

export interface UpdateElectionCommunicationConfigurationsRequest {
  id: string;
  confirmationOfRegistrationCommunicationTemplateId?: string;
  confirmationOfCancellationCommunicationTemplateId?: string;
  invitationCommunicationTemplateId?: string;
  invitationReminderCommunicationTemplateId?: string;
  taskReminderCommunicationTemplateId?: string;
  retractedInvitationCommunicationTemplateId?: string;
  removedFromTaskCommunicationTemplateId?: string;
  removedByValidationCommunicationTemplateId?: string;
  electionTaskTypeCommunicationTemplates?: Array<UpdateElectionTaskTypeCommunicationTemplateRequest>;
}
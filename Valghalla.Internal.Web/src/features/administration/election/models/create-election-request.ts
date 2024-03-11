export interface CreateElectionTaskTypeCommunicationTemplateRequest {
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

export interface CreateElectionRequest {
  title: string;
  electionTypeId: string;
  lockPeriod: number;
  electionStartDate: Date;
  electionEndDate: Date;
  electionDate: Date;
  workLocationIds: string[];
  confirmationOfRegistrationCommunicationTemplateId?: string;
  confirmationOfCancellationCommunicationTemplateId?: string;
  invitationCommunicationTemplateId?: string;
  invitationReminderCommunicationTemplateId?: string;
  taskReminderCommunicationTemplateId?: string;
  retractedInvitationCommunicationTemplateId?: string;
  removedFromTaskCommunicationTemplateId?: string;
  removedByValidationCommunicationTemplateId?: string;
  electionTaskTypeCommunicationTemplates?: Array<CreateElectionTaskTypeCommunicationTemplateRequest>;
}
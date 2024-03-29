export interface DuplicateElectionTaskTypeCommunicationTemplateRequest {
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

export interface DuplicateElectionRequest {
  sourceElectionId: string;
  title: string;
  electionTypeId: string;
  lockPeriod: number;
  electionDate: Date;
  daysBeforeElectionDate: number;
  daysAfterElectionDate: number;  
  workLocationIds: string[];
  confirmationOfRegistrationCommunicationTemplateId?: string;
  confirmationOfCancellationCommunicationTemplateId?: string;
  invitationCommunicationTemplateId?: string;
  invitationReminderCommunicationTemplateId?: string;
  taskReminderCommunicationTemplateId?: string;
  retractedInvitationCommunicationTemplateId?: string;
  removedFromTaskCommunicationTemplateId?: string;
  removedByValidationCommunicationTemplateId?: string;
  electionTaskTypeCommunicationTemplates?: Array<DuplicateElectionTaskTypeCommunicationTemplateRequest>;
}
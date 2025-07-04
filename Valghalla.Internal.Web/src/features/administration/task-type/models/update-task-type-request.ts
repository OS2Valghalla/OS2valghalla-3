export interface UpdateTaskTypeRequest {
  id: string;
  title: string;
  shortName: string;
  description: string;
  startTime: string;
  endTime: string;
  payment?: number;
  validationNotRequired: boolean;
  trusted: boolean;
  sendingReminderEnabled: boolean;
  fileReferenceIds: string[];
  newWorkLocationId?: string;
  workLocationId?: string;
  newElectionId?: string;
  electionId?: string;
  newTaskTypeTemplateId?: string;
  taskTypeTemplateId?: string;
}

export interface CreateTaskTypeRequest {
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
}

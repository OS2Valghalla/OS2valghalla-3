import { FileReference } from "src/shared/models/file-storage/file-reference";

export interface TaskTypeDetails {
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
  fileReferences: FileReference[];
  electionId: string;
  workLocationId: string;
  taskTypeTemplateId: string;
}

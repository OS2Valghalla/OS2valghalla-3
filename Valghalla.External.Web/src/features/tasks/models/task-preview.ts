import { FileReference } from 'src/shared/models/file-reference';

export interface TaskPreview {
  hashValue: string;
  taskDate: string;
  team: TaskPreviewTeam;
  taskType: TaskPreviewTaskType;
  workLocation: TaskPreviewWorkLocation;
}

export interface TaskDetails {
  hashValue: string;
  taskAssignmentId: string;
  accepted: boolean;
  isLocked: boolean;
  taskDate: string;
  team: TaskPreviewTeam;
  taskType: TaskTypeIncludeFiles;
  workLocation: TaskPreviewWorkLocation;
}

interface TaskPreviewTeam {
  id: string;
  name: string;
}

interface TaskPreviewTaskType {
  id: string;
  title: string;
  description: string;
  startTime: string;
  endTime: string;
  payment?: number;
}

interface TaskTypeIncludeFiles {
  id: string;
  title: string;
  description: string;
  startTime: string;
  endTime: string;
  payment?: number;
  fileReferences?: FileReference[];
}

interface TaskPreviewWorkLocation {
  id: string;
  title: string;
  address: string;
  postalCode: string;
  city: string;
}

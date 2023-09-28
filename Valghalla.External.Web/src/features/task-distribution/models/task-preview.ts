export interface TaskPreview {
  hashValue: string;
  taskDate: string;
  team: TaskPreviewTeam;
  taskType: TaskPreviewTaskType;
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
  trusted?: boolean;
  acceptedTasksCount: number;
  unansweredTasksCount: number;
  allTasksCount: number;
}

interface TaskPreviewWorkLocation {
  id: string;
  title: string;
  address: string;
  postalCode: string;
  city: string;
}

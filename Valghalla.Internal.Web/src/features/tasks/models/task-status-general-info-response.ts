import { RejectedTasksInfoResponse } from './rejected-tasks-info-response';

export interface TaskStatusGeneralInfoResponse {
  assignedTasksCount: number;
  missingTasksCount: number;
  awaitingTasksCount: number;
  allTasksCount: number;
  rejectedTasksCount: number;
  rejectedTasksInfoResponses: RejectedTasksInfoResponse[];
}

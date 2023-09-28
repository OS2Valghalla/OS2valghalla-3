export interface TasksSummary {
    tasksDate: Date;
    workLocationId: string;
    teamId: string;
    taskTypeId: string;
    assignedTasksCount: number;
    awaitingTasksCount: number;
    missingTasksCount: number;
    allTasksCount: number;
}
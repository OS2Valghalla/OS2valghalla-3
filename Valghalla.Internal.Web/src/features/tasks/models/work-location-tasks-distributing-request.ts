export interface TasksDistributingRequest {
    tasksDate: Date;
    teamId: string;
    taskTypeId: string;
    tasksCount: number;
}

export interface WorkLocationTasksDistributingRequest {
    electionId: string;
    workLocationId: string;
    distributingTasks: Array<TasksDistributingRequest>;
}
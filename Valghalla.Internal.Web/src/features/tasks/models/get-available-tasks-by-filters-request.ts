export interface TasksFilterRequest {
    teamId: string;
    taskDate?: Date;
    areaIds?: Array<string>;
    workLocationIds?: Array<string>;
    taskTypeIds?: Array<string>;
    trustedTaskType?: boolean;
}

export interface GetAvailableTasksByFiltersRequest {
    electionId: string;
    tasksFilter: TasksFilterRequest;
}
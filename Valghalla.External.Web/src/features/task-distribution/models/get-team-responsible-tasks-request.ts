export interface GetTeamResponsibleTasksRequest {
    teamId: string;
    workLocationId?: string;
    taskTypeId?: string;
    taskDate?: Date;
}
  
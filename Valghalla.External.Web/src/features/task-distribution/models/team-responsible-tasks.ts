export interface TeamResponsibleTasks {
    totalUnansweredTasksCount: number;
    totalAcceptedTasksCount: number;
    totalTasksCount: number;
    tasks: Array<TeamResponsibleTaskDetails>;
}

export interface TeamResponsibleTaskDetails {
    electionId: string;
    electionName: string;
    workLocationId: string;
    workLocationName: string;
    workLocationAddress: string;
    workLocationPostalCode: string;
    workLocationCity: string;
    taskTypeId: string;
    trustedTaskType: boolean;
    taskTypeName: string;
    taskTypeDescription: string;
    taskTypeStartTime: string;
    taskDate: Date;
    unansweredTasksCount: number;
    acceptedTasksCount: number;
    allTasksCount: number;
    hashValue: string;
}
  
export interface TeamResponsibleTasksFiltersOptions {
    teams: Array<TeamResponsibleTaskTeam>;
    taskTypes: Array<TeamResponsibleTaskTaskType>;
    workLocations: Array<TeamResponsibleTaskWorkLocation>;
}

export interface TeamResponsibleTaskTeam {
    id: string;
    name: string;
}
  
export interface TeamResponsibleTaskTaskType {
    id: string;
    title: string;
    description: string;
    startTime: string;
    endTime: string;
    payment?: number;
}

export interface TeamResponsibleTaskWorkLocation {
    id: string;
    title: string;
    address: string;
    postalCode: string;
    city: string;
}

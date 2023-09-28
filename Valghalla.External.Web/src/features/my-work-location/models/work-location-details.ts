export interface WorkLocationDate {
    taskDate: Date;
    detailsLoaded?: boolean;
    taskDetails?: WorkLocationTasksDetails;
}

export interface WorkLocationTasksDetails {
    acceptedTasksCount: number;
    allTasksCount: number;
    taskTypes: Array<WorkLocationTaskTypeDetails>;
    participants: Array<WorkLocationParticipantDetails>;
}

export interface WorkLocationTaskTypeDetails {
    id: string;
    title: string;
    startTime: string;
    endTime: string;
    acceptedTasksCount: number;
    allTasksCount: number;
}

export interface WorkLocationParticipantDetails {
    fullName: string;
    email: string;
    mobileNumber: string;
    age: number;
    taskTypes: string;
    teams: string;
}
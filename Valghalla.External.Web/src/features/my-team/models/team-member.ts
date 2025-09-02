export interface TeamMemberTaskInfo {
    taskTitle: string;
    taskStatus: TaskStatus;
    taskDate?: string;
}

export interface TeamMemberWorkLocationInfo {
    workLocationTitle: string;
    tasks: Array<TeamMemberTaskInfo>;
}

export enum TaskStatus {
    Accepted = 0,
    Unanswered = 1,
    Rejected = 2,
    Available = 3
}

export interface TeamMember {
    id: string;
    name: string;
    assignedTasksCount: number;
    workLocations: Array<TeamMemberWorkLocationInfo>;
    canBeRemoved: boolean;
}
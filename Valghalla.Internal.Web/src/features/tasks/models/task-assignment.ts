export interface TaskAssignment {
    id: string;    
    electionId: string;
    workLocationId: string;
    workLocationName: string;
    teamId: string;
    teamName: string;
    taskTypeId: string;
    taskTypeName: string;
    participantId?: string;
    participantName?: string;
    taskDate: Date;
    accepted: boolean;
    responsed: boolean;
}
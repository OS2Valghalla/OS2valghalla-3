export interface ParticipantTask {
    id: string;    
    electionId: string;
    electionName: string;
    workLocationId: string;
    workLocationName: string;
    teamId: string;
    teamName: string;
    taskTypeId: string;
    taskTypeName: string;
    taskDate: Date;
    status: string;
}
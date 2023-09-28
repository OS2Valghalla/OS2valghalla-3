export interface AssignParticipantToTaskRequest {
    electionId: string;
    taskAssignmentId: string;
    participantId: string;
    taskTypeId: string;
}
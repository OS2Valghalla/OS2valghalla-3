export interface ReplyForParticipantRequest {
    electionId: string;
    taskAssignmentId: string;
    accepted: boolean;
    mobileNumber?: string;
    email?: string;
    specialDietIds: string[];
}
  
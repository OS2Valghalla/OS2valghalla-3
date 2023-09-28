export interface CreateParticipantRequest {
  cpr: string;
  mobileNumber?: string;
  email?: string;
  specialDietIds: string[];
  teamIds: string[];
  electionId?: string;
  taskId?: string;
}

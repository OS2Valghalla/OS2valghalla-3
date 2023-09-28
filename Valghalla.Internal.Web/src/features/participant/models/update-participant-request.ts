export interface UpdateParticipantRequest {
  id: string;
  mobileNumber?: string;
  email?: string;
  specialDietIds: string[];
  teamIds: string[];
}

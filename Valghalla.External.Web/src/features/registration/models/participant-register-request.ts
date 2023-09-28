export interface ParticipantRegisterRequest {
  hashValue: string;
  mobileNumber?: string;
  email?: string;
  specialDietIds: string[];
}

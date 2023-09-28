import { ParticipantPersonalRecord } from "./participant-personal-record";

export interface ParticipantDetails extends ParticipantPersonalRecord {
  id: string;
  cpr: string;
  mobileNumber?: string;
  email?: string;
  specialDietIds: string[];
  memberTeamIds: string[];
}

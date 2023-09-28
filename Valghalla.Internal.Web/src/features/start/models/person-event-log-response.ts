export interface PersonEventLogResponse {
  id: string;
  electionId: string;
  text: string;
  created: Date;
  date: Date;
  createdBy: string;
  personFirstName: string;
  personLastName: string;
  personId: string;
  inactive: boolean;
  hiddenIdentity: boolean;
}

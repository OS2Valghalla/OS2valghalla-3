export interface ElectionDetails {
  id: string;
  electionTypeId: string;
  title: string;
  lockPeriod: number;
  electionStartDate: Date;
  electionEndDate: Date;
  electionDate: Date;
  active: boolean;
  workLocationIds: string[];
}

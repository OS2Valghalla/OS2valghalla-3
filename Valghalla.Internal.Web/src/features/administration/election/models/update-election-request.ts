export interface UpdateElectionRequest {
  id: string;
  title: string;
  lockPeriod: number;
  workLocationIds: string[];
}
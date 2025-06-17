export interface WorkLocationDetails {
  id: string;
  title: string;
  areaId: string;
  address: string;
  postalCode: string;
  city: string;
  voteLocation: number;
  hasActiveElection?: boolean;
  taskTypeIds: Array<string>;
  taskTypeTemplateIds: Array<string>;
  teamIds: Array<string>;
  responsibleIds: Array<string>;
  electionId?: string;
}
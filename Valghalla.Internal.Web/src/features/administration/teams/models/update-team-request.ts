export interface UpdateTeamRequest {
    id: string;
    name: string;
    shortName: string;
    description?: string;
    responsibleIds?: Array<string>;
  }
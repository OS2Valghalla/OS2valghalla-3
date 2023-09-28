export interface CreateTeamRequest {
    name: string;
    shortName: string;
    description?: string;
    responsibleIds?: Array<string>;
  }
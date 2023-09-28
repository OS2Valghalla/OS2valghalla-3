export interface WorkLocationDetails {
    id: string;
    title: string;
    areaId: string;
    address: string;
    postalCode: string;
    city: string;
    hasActiveElection?: boolean;
    taskTypeIds: Array<string>;
    teamIds: Array<string>;
    responsibleIds: Array<string>;
  }
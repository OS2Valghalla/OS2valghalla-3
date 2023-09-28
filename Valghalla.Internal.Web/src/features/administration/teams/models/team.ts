export interface Team {
    id: string;
    name: string;
    shortName: string;
    description?: string;
    responsibleCount?: number;
    taskCount?: number;
    responsibleIds?: string[];
  }
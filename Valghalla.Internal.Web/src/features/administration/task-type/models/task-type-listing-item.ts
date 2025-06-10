export interface TaskTypeListingItem {
  id: string;
  title: string;
  shortName: string;
  trusted: boolean;
  areaName: string;
  areaId: string;
  electionId?: string;
  taskTypeTemplateId?: string;
  electionTitle?: string;
  taskTypeTemplateTitle?: string;
}

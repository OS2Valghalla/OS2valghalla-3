export interface ParticipantListingItem {
  id: string;
  name: string;
  birthdate: Date;
  digitalPost: boolean;
  assignedTask: boolean;
  hasUnansweredTask: boolean;
  teamIds: string[];
  taskTypeIds: string[];
}

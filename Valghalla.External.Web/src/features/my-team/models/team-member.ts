export interface TeamMember {
    id: string;
    name: string;
    assignedTasksCount: number;
    workLocations: string;
    canBeRemoved: boolean;
}
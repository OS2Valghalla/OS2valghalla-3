export interface TeamMember {
    id: string;
    name: string;
    assignedTasksCount: number;
    canBeRemoved: boolean;
}
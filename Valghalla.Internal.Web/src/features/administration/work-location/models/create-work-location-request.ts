export interface CreateWorkLocationRequest {
    title: string;
    areaId: string;
    address: string;
    postalCode: string;
    city: string;
    taskTypeIds: Array<string>;
    teamIds: Array<string>;
    responsibleIds?: Array<string>;
}
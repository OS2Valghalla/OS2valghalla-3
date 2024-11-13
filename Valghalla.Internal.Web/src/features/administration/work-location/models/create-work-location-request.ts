export interface CreateWorkLocationRequest {
    title: string;
    areaId: string;
    address: string;
    postalCode: string;
    city: string;
    voteLocation: Number;
    taskTypeIds: Array<string>;
    teamIds: Array<string>;
    responsibleIds?: Array<string>;
}
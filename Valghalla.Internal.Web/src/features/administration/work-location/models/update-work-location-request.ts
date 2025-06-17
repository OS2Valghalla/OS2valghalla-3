export interface UpdateWorkLocationRequest {
    id: string;
    title: string;
    areaId: string;
    address: string;
    postalCode: string;
    city: string;
    voteLocation: Number;
    taskTypeIds: Array<string>;
    taskTypeTemplateIds: Array<string>;
    teamIds: Array<string>;
    responsibleIds?: Array<string>;
}
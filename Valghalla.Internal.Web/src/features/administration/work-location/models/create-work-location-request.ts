import { CreateTaskTypeTemplateRequest } from "../../task-type-template/models/create-task-type-template-request";
import { CreateTaskTypeRequest } from "../../task-type/models/create-task-type-request";

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
    taskTypeTemplateIds: Array<string>;
    electionId?: string;
}
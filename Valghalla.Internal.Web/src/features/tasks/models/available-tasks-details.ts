export interface AvailableTasksDetails {
    electionId: string;
    workLocationId: string;
    workLocationName: string;
    workLocationAddress: string;
    workLocationPostalCode: string;
    workLocationCity: string;
    taskTypeId: string;
    taskTypeName: string;
    taskTypeDescription: string;
    taskTypeStartTime: string;
    taskDate: Date;
    trustedTask: boolean;
    availableTasksCount: number;
    taskDetailsPageUrl: string;
}
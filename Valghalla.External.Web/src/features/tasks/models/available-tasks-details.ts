export interface AvailableTasksDetails {
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
    availableTasksCount: number;
    hashValue: string;
}
import { UnprotectedTasksFilter } from './unprotected-tasks-filter';

export interface GetUnprotectedAvailableTasksByFiltersRequest {
    hashValue: string;
    tasksFilter: UnprotectedTasksFilter;
}
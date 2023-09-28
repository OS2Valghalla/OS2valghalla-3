import { WorkLocationShared } from 'src/shared/models/work-location-shared';
import { UnprotectedTasksFilter } from './unprotected-tasks-filter';

export interface TasksFiltersOptions {
    taskDates: Array<Date>;
    workLocations: Array<WorkLocationShared>;
    tasksFilter: UnprotectedTasksFilter;
}
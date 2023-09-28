import { TaskTypeShared } from 'src/shared/models/task-type-shared';
import { TeamShared } from 'src/shared/models/team-shared';
import { WorkLocationShared } from 'src/shared/models/work-location/work-location-shared';
import { TasksSummary } from './tasks-summary';

export interface AreaTasksSummary {
    electionStartDate: Date;
    electionEndDate: Date;
    workLocations: Array<WorkLocationShared>;
    taskTypes: Array<TaskTypeShared>;
    teams: Array<TeamShared>;
    tasks: Array<TasksSummary>;
}
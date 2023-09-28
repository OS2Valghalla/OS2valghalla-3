import { AreaShared } from 'src/shared/models/area/area-shared';
import { TaskTypeShared } from 'src/shared/models/task-type/task-type-shared';
import { TeamShared } from 'src/shared/models/team/team-shared';
import { WorkLocationShared } from 'src/shared/models/work-location/work-location-shared';

export interface WorkLocationWithTeamIdsResponse extends WorkLocationShared {
    teamIds: Array<string>;
  }

export interface TaskTypeWithTeamIdsResponse extends TaskTypeShared {
  teamIds: Array<string>;
}

export interface TasksFiltersOptions {
  electionStartDate: Date;
  electionEndDate: Date;
  areas: Array<AreaShared>;
  workLocations: Array<WorkLocationWithTeamIdsResponse>;
  taskTypes: Array<TaskTypeWithTeamIdsResponse>;
  teams: Array<TeamShared>;
}

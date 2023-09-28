import { AreaShared } from 'src/shared/models/area/area-shared';
import { TaskTypeShared } from 'src/shared/models/task-type/task-type-shared';
import { TeamShared } from 'src/shared/models/team/team-shared';
import { WorkLocationShared } from 'src/shared/models/work-location/work-location-shared';

export interface TaskTypeWithAreaIdsResponse extends TaskTypeShared {
  areaIds: Array<string>;
}

export interface ElectionAreasGeneralInfo {
  electionStartDate: Date;
  electionEndDate: Date;
  areas: Array<AreaShared>;
  workLocations: Array<WorkLocationShared>;
  taskTypes: Array<TaskTypeWithAreaIdsResponse>;
  teams: Array<TeamShared>;
}

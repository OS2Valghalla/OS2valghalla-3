import { TaskTypeShared } from 'src/shared/models/task-type/task-type-shared';
import { TasksSummary } from './tasks-summary';
import { TeamShared } from 'src/shared/models/team/team-shared';

export interface WorkLocationTasksSummary {
  electionStartDate: Date;
  electionEndDate: Date;
  electionDate: Date;
  taskTypes: Array<TaskTypeShared>;
  teams: Array<TeamShared>;
  tasks: Array<TasksSummary>;
}

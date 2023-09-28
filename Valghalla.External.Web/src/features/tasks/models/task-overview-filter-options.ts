import { SelectOption } from 'src/shared/query-engine/models/select-option';

export interface TaskOverviewFilterOptions {
  taskDates: Array<Date>;
  workLocations: SelectOption<string>[];
  taskTypes: SelectOption<string>[];
  teams: SelectOption<string>[];
}

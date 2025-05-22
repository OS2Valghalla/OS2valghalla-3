import { Component, OnInit, AfterViewInit, ViewChild } from '@angular/core';
import { SubSink } from 'subsink';
import { RoutingNodes } from 'src/shared/constants/routing-nodes';
import { AreaSharedHttpService } from 'src/shared/services/area-shared-http.service';
import { GlobalStateService } from 'src/app/global-state.service';
import { AreaTasksHttpService } from '../../services/area-tasks-http.service';
import { TasksSummary } from '../../models/tasks-summary';
import { ElectionAreasGeneralInfo, TaskTypeWithAreaIdsResponse } from '../../models/election-areas-general-info';
import { ElectionShared } from 'src/shared/models/election/election-shared';
import { MatSelectionList } from '@angular/material/list';

export interface AreaTasksSummary {
  areaId: string;
  workLocations: Array<WorkLocationTasksSummary>;
  displayedColumns: Array<string>;
}

interface TableDisplayColumnOptions {
  id: string;
  displayName: string;
}
interface TableDisplayColumnSettings {
  options: Array<TableDisplayColumnOptions>;
  selected: Array<string>;
}
export interface WorkLocationTasksSummary {
  workLocationId: string;
  workLocationName: string;
  taskTypes: Array<TaskTypeTasksSummary>;
}

export interface TaskTypeTasksSummary {
  taskTypeId: string;
  assignedTasksCount: number;
  awaitingTasksCount: number;
  missingTasksCount: number;
  allTasksCount: number;
}

export interface StatusesTasksSummary {
  statusName: string;
  taskTypes: Array<TaskTypeTasksSummary>;
  sumAssignedTasksCount: number;
  sumAwaitingTasksCount: number;
  sumMissingTasksCount: number;
  sumAllTasksCount: number;
}

@Component({
  selector: 'app-tasks-overview',
  templateUrl: './tasks-overview.component.html',
  styleUrls: ['../../../../shared/components/table/table.component.scss', 'tasks-overview.component.scss'],
  providers: [AreaSharedHttpService, AreaTasksHttpService],
})
export class TasksOverviewComponent implements AfterViewInit {

  private readonly subs = new SubSink();

  loadingAreas = true;

  loadingTasks = true;

  election?: ElectionShared;

  areasGeneralInfo?: ElectionAreasGeneralInfo;

  electionDates: Array<Date> = [];

  selectedDate?: Date;

  selectedTeamId?: string;

  taskDates: Array<Date>;

  areaTasksSummary: Array<TasksSummary> = [];

  data: Array<AreaTasksSummary> = [];

  displayedColumns: Array<string> = ['status'];

  displayedColumnsOptions: Array<TableDisplayColumnOptions> = [];

  displayedColumnsOptionsSessionKey = 'tasks-overview-columns';

  selectedColumns: Array<string> = [];

  workLocationLink: string;

  allAreasData: Array<StatusesTasksSummary>;

  @ViewChild('columnsList') columnsList: MatSelectionList;

  allColumnsSelected = false;

  columns = [
    {
      name: 'participantName',
      key: 'list.participant_list.labels.full_name',
      displayName: 'list.participant_list.labels.full_name',
      index: 1,
      disabled: true,
      isSelected: true,
    }
  ];

  constructor(private globalStateService: GlobalStateService, private areaTasksHttpService: AreaTasksHttpService) { }

  ngAfterViewInit(): void {
    this.workLocationLink = '/' + RoutingNodes.TasksOnWorkLocation + '/';
    this.subs.sink = this.globalStateService.election$.subscribe((election) => {
      this.election = election;

      if (this.election) {
        this.loadingAreas = true;
        this.loadingTasks = true;
        this.displayedColumns = ['status'];
        this.electionDates = [];
        this.selectedDate = undefined;
        this.selectedTeamId = undefined;

        this.subs.sink = this.areaTasksHttpService.getAreasGeneralInfo(this.election.id).subscribe((resAreas) => {
          this.areasGeneralInfo = resAreas.data;
          const tasksDate: Date = new Date(this.areasGeneralInfo.electionStartDate);
          while (tasksDate <= new Date(this.areasGeneralInfo.electionEndDate)) {
            this.electionDates.push(new Date(tasksDate));
            tasksDate.setDate(tasksDate.getDate() + 1);
          }
          this.areasGeneralInfo.taskTypes.forEach((taskType) => {
            this.displayedColumns.push(taskType.id);
          });
          this.displayedColumns.push('total');

          this.loadingAreas = false;
          this.subs.sink = this.areaTasksHttpService.getAreaTasksSummary(this.election.id).subscribe((res) => {
            this.areaTasksSummary = res.data;
            this.buildAreaTasksSummary();
            this.initializeTableDisplayFromSession();
          });
        });
      }
    });
  }

  private saveColumnSettingsToStorage(options: TableDisplayColumnOptions[], selected: string[]) {
    sessionStorage.setItem(this.displayedColumnsOptionsSessionKey, JSON.stringify({
      options,
      selected
    }));
  }

  private loadColumnSettingsFromStorage(validOptionIds: string[]): string[] {
    const sessionValue = sessionStorage.getItem(this.displayedColumnsOptionsSessionKey);
    if (sessionValue) {
      try {
        const parsed = JSON.parse(sessionValue) as TableDisplayColumnSettings;
        return Array.isArray(parsed.selected)
          ? parsed.selected.filter(id => validOptionIds.includes(id))
          : [];
      } catch {
        return [...validOptionIds];
      }
    }
    return [...validOptionIds];
  }

  private initializeTableDisplayFromSession() {
    this.displayedColumnsOptions = this.areasGeneralInfo.taskTypes.map(taskType => ({
      id: taskType.id,
      displayName: taskType.shortName
    }));

    this.displayedColumnsOptions.push({
      id: 'total',
      displayName: 'TOTAL',
    });

    const validOptionIds = this.displayedColumnsOptions.map(opt => opt.id);

    this.selectedColumns = this.loadColumnSettingsFromStorage(validOptionIds);

    this.displayedColumns = ['status', ...this.selectedColumns];
    this.data.forEach((areaSummary) => {
      areaSummary.displayedColumns = ['workLocation', ...this.selectedColumns];
    });
    this.allColumnsSelected = this.selectedColumns.length === validOptionIds.length;

    this.saveColumnSettingsToStorage(this.displayedColumnsOptions, this.selectedColumns);
  }

  onFilterChanged() {
    this.loadingTasks = true;
    this.subs.sink = this.areaTasksHttpService
      .getAreaTasksSummary(this.election.id, this.selectedDate, this.selectedTeamId)
      .subscribe((res) => {
        this.areaTasksSummary = res.data;
        this.buildAreaTasksSummary();
      });
  }

  buildAreaTasksSummary() {
    this.data = [];
    this.allAreasData = [
      {
        statusName: 'all',
        taskTypes: [],
        sumAssignedTasksCount: 0,
        sumAllTasksCount: 0,
        sumAwaitingTasksCount: 0,
        sumMissingTasksCount: 0,
      },
      {
        statusName: 'missing',
        taskTypes: [],
        sumAssignedTasksCount: 0,
        sumAllTasksCount: 0,
        sumAwaitingTasksCount: 0,
        sumMissingTasksCount: 0,
      },
      {
        statusName: 'awaiting',
        taskTypes: [],
        sumAssignedTasksCount: 0,
        sumAllTasksCount: 0,
        sumAwaitingTasksCount: 0,
        sumMissingTasksCount: 0,
      },
    ];

    this.areasGeneralInfo.areas.forEach((area) => {
      const areaTasksSummary: AreaTasksSummary = {
        areaId: area.id,
        workLocations: [],
        displayedColumns: ['workLocation'],
      };
      this.areasGeneralInfo.taskTypes.forEach((taskType) => {
        if (taskType.areaIds.indexOf(area.id) > -1) {
          areaTasksSummary.displayedColumns.push(taskType.id);
        }
      });
      areaTasksSummary.displayedColumns.push('total');
      const foundWorkLocations = this.areasGeneralInfo.workLocations.filter((x) => x.areaId == area.id);
      if (foundWorkLocations && foundWorkLocations.length > 0) {
        foundWorkLocations.forEach((workLocation) => {
          const workLocationTasksSummary: WorkLocationTasksSummary = {
            workLocationId: workLocation.id,
            workLocationName: workLocation.title,
            taskTypes: [],
          };
          this.areasGeneralInfo.taskTypes.forEach((taskType, taskTypeIndex) => {
            const foundTasks = this.areaTasksSummary.filter(
              (t) => t.workLocationId == workLocation.id && t.taskTypeId == taskType.id,
            );
            const taskTypeTasksSummary: TaskTypeTasksSummary = {
              taskTypeId: taskType.id,
              assignedTasksCount: foundTasks && foundTasks.length > 0 ? foundTasks[0].assignedTasksCount : 0,
              awaitingTasksCount: foundTasks && foundTasks.length > 0 ? foundTasks[0].awaitingTasksCount : 0,
              missingTasksCount: foundTasks && foundTasks.length > 0 ? foundTasks[0].missingTasksCount : 0,
              allTasksCount: foundTasks && foundTasks.length > 0 ? foundTasks[0].allTasksCount : 0,
            };
            workLocationTasksSummary.taskTypes.push(taskTypeTasksSummary);

            this.allAreasData[0].sumAssignedTasksCount +=
              foundTasks && foundTasks.length > 0 ? foundTasks[0].assignedTasksCount : 0;
            this.allAreasData[0].sumAllTasksCount +=
              foundTasks && foundTasks.length > 0 ? foundTasks[0].allTasksCount : 0;
            this.allAreasData[1].sumMissingTasksCount +=
              foundTasks && foundTasks.length > 0 ? foundTasks[0].missingTasksCount : 0;
            this.allAreasData[2].sumAwaitingTasksCount +=
              foundTasks && foundTasks.length > 0 ? foundTasks[0].awaitingTasksCount : 0;

            if (this.allAreasData[0].taskTypes.length <= taskTypeIndex) {
              this.allAreasData[0].taskTypes.push({
                taskTypeId: taskType.id,
                assignedTasksCount: foundTasks && foundTasks.length > 0 ? foundTasks[0].assignedTasksCount : 0,
                allTasksCount: foundTasks && foundTasks.length > 0 ? foundTasks[0].allTasksCount : 0,
                awaitingTasksCount: 0,
                missingTasksCount: 0,
              });
              this.allAreasData[1].taskTypes.push({
                taskTypeId: taskType.id,
                assignedTasksCount: 0,
                allTasksCount: 0,
                awaitingTasksCount: 0,
                missingTasksCount: foundTasks && foundTasks.length > 0 ? foundTasks[0].missingTasksCount : 0,
              });
              this.allAreasData[2].taskTypes.push({
                taskTypeId: taskType.id,
                assignedTasksCount: 0,
                allTasksCount: 0,
                awaitingTasksCount: foundTasks && foundTasks.length > 0 ? foundTasks[0].awaitingTasksCount : 0,
                missingTasksCount: 0,
              });
            } else if (foundTasks && foundTasks.length > 0) {
              this.allAreasData[0].taskTypes[taskTypeIndex].allTasksCount += foundTasks[0].allTasksCount;
              this.allAreasData[0].taskTypes[taskTypeIndex].assignedTasksCount += foundTasks[0].assignedTasksCount;
              this.allAreasData[1].taskTypes[taskTypeIndex].missingTasksCount += foundTasks[0].missingTasksCount;
              this.allAreasData[2].taskTypes[taskTypeIndex].awaitingTasksCount += foundTasks[0].awaitingTasksCount;
            }
          });
          areaTasksSummary.workLocations.push(workLocationTasksSummary);
        });
      }
      this.data.push(areaTasksSummary);
    });
    this.loadingTasks = false;
  }

  getWorkLocationAssignedTasksCount(workLocation: WorkLocationTasksSummary) {
    return workLocation.taskTypes.map((t) => t.assignedTasksCount).reduce((acc, value) => acc + value, 0);
  }

  getWorkLocationAllTasksCount(workLocation: WorkLocationTasksSummary) {
    return workLocation.taskTypes.map((t) => t.allTasksCount).reduce((acc, value) => acc + value, 0);
  }

  getTaskTypeAssignedTasksCount(workLocations: Array<WorkLocationTasksSummary>, taskTypeId: string) {
    let assignedTasksCount = 0;
    workLocations.forEach((workLocation) => {
      const foundTaskTypes = workLocation.taskTypes.filter((t) => t.taskTypeId == taskTypeId);
      if (foundTaskTypes && foundTaskTypes.length > 0) {
        assignedTasksCount += foundTaskTypes[0].assignedTasksCount;
      }
    });
    return assignedTasksCount;
  }

  getTaskTypeAllTasksCount(workLocations: Array<WorkLocationTasksSummary>, taskTypeId: string) {
    let allTasksCount = 0;
    workLocations.forEach((workLocation) => {
      const foundTaskTypes = workLocation.taskTypes.filter((t) => t.taskTypeId == taskTypeId);
      if (foundTaskTypes && foundTaskTypes.length > 0) {
        allTasksCount += foundTaskTypes[0].allTasksCount;
      }
    });
    return allTasksCount;
  }

  getSumAssignedTasksCount(workLocations: Array<WorkLocationTasksSummary>) {
    let assignedTasksCount = 0;
    workLocations.forEach((workLocation) => {
      workLocation.taskTypes.forEach((taskType) => {
        assignedTasksCount += taskType.assignedTasksCount;
      });
    });
    return assignedTasksCount;
  }

  getSumAllTasksCount(workLocations: Array<WorkLocationTasksSummary>) {
    let allTasksCount = 0;
    workLocations.forEach((workLocation) => {
      workLocation.taskTypes.forEach((taskType) => {
        allTasksCount += taskType.allTasksCount;
      });
    });
    return allTasksCount;
  }

  toggleAllColumns() {
    if (this.allColumnsSelected) {
      this.columnsList.options.forEach((item) => {
        if (!item.disabled) item.selected = true;
      });
    } else {
      this.columnsList.options.forEach((item) => {
        if (!item.disabled) item.selected = false;
      });
    }
    this.changeSelectedColumns();
  }

  changeSelectedColumns() {
    const selectedOptions: string[] = this.columnsList
      ? this.columnsList.selectedOptions.selected.map(item => item.value)
      : [...this.selectedColumns];

    this.allColumnsSelected = selectedOptions.length === this.displayedColumnsOptions.length;

    const sorted = selectedOptions.sort((a, b) => {
      const aIndex = this.columns.findIndex(col => col.name === a);
      const bIndex = this.columns.findIndex(col => col.name === b);
      return aIndex - bIndex;
    });

    this.displayedColumns = ['status', ...sorted];

    this.data.forEach((areaSummary) => {
      areaSummary.displayedColumns = ['workLocation', ...sorted];
    });

    this.saveColumnSettingsToStorage(this.displayedColumnsOptions, sorted);
  }

  isColumnDisabled(columnName: string): boolean {
    const column = this.columns.find(col => col.name === columnName);
    return column ? column.disabled : false;
  }

}

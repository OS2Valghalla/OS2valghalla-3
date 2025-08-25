import { Component, AfterViewInit, ViewChild } from '@angular/core';
import { SubSink } from 'subsink';
import { RoutingNodes } from 'src/shared/constants/routing-nodes';
import { AreaSharedHttpService } from 'src/shared/services/area-shared-http.service';
import { GlobalStateService } from 'src/app/global-state.service';
import { AreaTasksHttpService } from '../../services/area-tasks-http.service';
import { TasksSummary } from '../../models/tasks-summary';
import { ElectionAreasGeneralInfo, TaskTypeWithAreaIdsResponse } from '../../models/election-areas-general-info';
import { ElectionShared } from 'src/shared/models/election/election-shared';
import { MatSelectionList } from '@angular/material/list';
import { Router } from '@angular/router';
import { TaskStatusGeneralInfoResponse } from '../../models/task-status-general-info-response';
import { TranslocoService } from '@ngneat/transloco';

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
  rejectedTasksCount?: number;
}

export interface StatusesTasksSummary {
  statusName: string;
  taskTypes: Array<TaskTypeTasksSummary>;
  sumAssignedTasksCount: number;
  sumAwaitingTasksCount: number;
  sumMissingTasksCount: number;
  sumAllTasksCount: number;
  sumRejectedTasksCount: number;
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

  selectedDates: Date[] = [];

  selectedTeamIds: string[] = [];

  taskDates: Array<Date>;

  areaTasksSummary: Array<TasksSummary> = [];

  data: Array<AreaTasksSummary> = [];

  tasksStatusGeneralInfo: TaskStatusGeneralInfoResponse;

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

  constructor(private globalStateService: GlobalStateService,
    private areaTasksHttpService: AreaTasksHttpService,
    private router: Router,
    private transloco: TranslocoService) { }

  ngAfterViewInit(): void {
    this.workLocationLink = '/' + RoutingNodes.TasksOnWorkLocation + '/';
    this.subs.sink = this.globalStateService.election$.subscribe((election) => {
      this.election = election;

      if (this.election) {
        this.loadingAreas = true;
        this.loadingTasks = true;
        this.displayedColumns = ['status'];
        this.electionDates = [];
        this.selectedDates = [];
        this.selectedTeamIds = [];

        this.subs.sink = this.areaTasksHttpService.getTasksStatusSummary(this.election.id).subscribe((res) => {
          this.tasksStatusGeneralInfo = res.data;
          this.tasksStatusGeneralInfo.rejectedTasksInfoResponses = this.tasksStatusGeneralInfo.rejectedTasksInfoResponses || [];
        });

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

  getSelectedDatesTooltip(): string {
    if (!this.selectedDates || this.selectedDates.length === 0) {
      return this.transloco.translate('tasks.labels.all_dates');
    }
    return this.selectedDates
      .map(d => d instanceof Date ? d.toLocaleDateString() : new Date(d).toLocaleDateString())
      .join(', ');
  }

  getSelectedTeamsTooltip(): string {
    if (!this.selectedTeamIds || this.selectedTeamIds.length === 0) {
      return this.transloco.translate('tasks.labels.all_teams');
    }
    if (!this.areasGeneralInfo?.teams) return '';
    const names = this.areasGeneralInfo.teams
      .filter(t => this.selectedTeamIds.includes(t.id))
      .map(t => t.name);
    return names.join(', ');
  }

  private saveColumnSettingsToStorage(options: TableDisplayColumnOptions[], selected: string[]) {
    sessionStorage.setItem(this.displayedColumnsOptionsSessionKey, JSON.stringify({ options, selected }));
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
    const dates = this.selectedDates && this.selectedDates.length > 0 ? this.selectedDates : undefined;
    const teams = this.selectedTeamIds && this.selectedTeamIds.length > 0 ? this.selectedTeamIds : undefined;
    this.subs.sink = this.areaTasksHttpService
      .getAreaTasksSummary(this.election.id, dates, teams)
      .subscribe((res) => {
        this.areaTasksSummary = res.data;
        this.buildAreaTasksSummary();
      });
  }

  buildAreaTasksSummary() {
    this.data = [];
    this.allAreasData = ['all', 'missing', 'awaiting', 'rejected'].map(status => ({
      statusName: status,
      taskTypes: [] as TaskTypeTasksSummary[],
      sumAssignedTasksCount: 0,
      sumAllTasksCount: 0,
      sumAwaitingTasksCount: 0,
      sumMissingTasksCount: 0,
      sumRejectedTasksCount: 0,
    }));

    this.areasGeneralInfo.areas.forEach(area => {
      const areaSummary: AreaTasksSummary = {
        areaId: area.id,
        workLocations: [],
        displayedColumns: ['workLocation']
      };
      this.areasGeneralInfo.taskTypes.forEach(tt => {
        if (tt.areaIds.includes(area.id)) areaSummary.displayedColumns.push(tt.id);
      });
      areaSummary.displayedColumns.push('total');

      const workLocations = this.areasGeneralInfo.workLocations.filter(wl => wl.areaId === area.id);
      workLocations.forEach(wl => {
        const wlSummary: WorkLocationTasksSummary = {
          workLocationId: wl.id,
          workLocationName: wl.title,
          taskTypes: []
        };
        this.areasGeneralInfo.taskTypes.forEach((tt, idx) => {
          const found = this.areaTasksSummary.find(t => t.workLocationId === wl.id && t.taskTypeId === tt.id);
          const taskSummary: TaskTypeTasksSummary = {
            taskTypeId: tt.id,
            assignedTasksCount: found?.assignedTasksCount || 0,
            awaitingTasksCount: found?.awaitingTasksCount || 0,
            missingTasksCount: found?.missingTasksCount || 0,
            allTasksCount: found?.allTasksCount || 0
          };
          wlSummary.taskTypes.push(taskSummary);

          this.allAreasData[0].sumAssignedTasksCount += taskSummary.assignedTasksCount;
          this.allAreasData[0].sumAllTasksCount += taskSummary.allTasksCount;
          this.allAreasData[1].sumMissingTasksCount += taskSummary.missingTasksCount;
          this.allAreasData[2].sumAwaitingTasksCount += taskSummary.awaitingTasksCount;

          if (this.allAreasData[0].taskTypes.length <= idx) {
            this.allAreasData[0].taskTypes.push({ taskTypeId: tt.id, assignedTasksCount: taskSummary.assignedTasksCount, allTasksCount: taskSummary.allTasksCount, awaitingTasksCount: 0, missingTasksCount: 0 });
            this.allAreasData[1].taskTypes.push({ taskTypeId: tt.id, assignedTasksCount: 0, allTasksCount: 0, awaitingTasksCount: 0, missingTasksCount: taskSummary.missingTasksCount });
            this.allAreasData[2].taskTypes.push({ taskTypeId: tt.id, assignedTasksCount: 0, allTasksCount: 0, awaitingTasksCount: taskSummary.awaitingTasksCount, missingTasksCount: 0 });
          } else {
            this.allAreasData[0].taskTypes[idx].allTasksCount += taskSummary.allTasksCount;
            this.allAreasData[0].taskTypes[idx].assignedTasksCount += taskSummary.assignedTasksCount;
            this.allAreasData[1].taskTypes[idx].missingTasksCount += taskSummary.missingTasksCount;
            this.allAreasData[2].taskTypes[idx].awaitingTasksCount += taskSummary.awaitingTasksCount;
          }
        });
        areaSummary.workLocations.push(wlSummary);
      });
      this.data.push(areaSummary);
    });

    if (this.tasksStatusGeneralInfo?.rejectedTasksInfoResponses && this.areasGeneralInfo) {
      const dateSet = (this.selectedDates && this.selectedDates.length > 0)
        ? new Set(this.selectedDates.map(d => new Date(d).toDateString()))
        : undefined;
      const teamSet = (this.selectedTeamIds && this.selectedTeamIds.length > 0)
        ? new Set(this.selectedTeamIds)
        : undefined;

      const filteredRejected = this.tasksStatusGeneralInfo.rejectedTasksInfoResponses.filter(r => {
        const dateOk = !dateSet || dateSet.has(new Date(r.tasksDate).toDateString());
        const teamOk = !teamSet || teamSet.has(r.teamId);
        return dateOk && teamOk;
      });

      const perTaskType = new Map<string, number>();
      filteredRejected.forEach(r => perTaskType.set(r.taskTypeId, (perTaskType.get(r.taskTypeId) || 0) + 1));

      this.areasGeneralInfo.taskTypes.forEach((tt, idx) => {
        const count = perTaskType.get(tt.id) || 0;
        if (this.allAreasData[3].taskTypes.length <= idx) {
          this.allAreasData[3].taskTypes.push({
            taskTypeId: tt.id,
            assignedTasksCount: 0,
            awaitingTasksCount: 0,
            missingTasksCount: 0,
            allTasksCount: 0,
            rejectedTasksCount: count
          });
        } else {
          this.allAreasData[3].taskTypes[idx].rejectedTasksCount = count;
        }
        this.allAreasData[3].sumRejectedTasksCount += count;
      });
    }

    if (this.tasksStatusGeneralInfo) {
      this.tasksStatusGeneralInfo.assignedTasksCount = this.allAreasData[0].sumAssignedTasksCount;
      this.tasksStatusGeneralInfo.allTasksCount = this.allAreasData[0].sumAllTasksCount;
      this.tasksStatusGeneralInfo.missingTasksCount = this.allAreasData[1].sumMissingTasksCount;
      this.tasksStatusGeneralInfo.awaitingTasksCount = this.allAreasData[2].sumAwaitingTasksCount;
      if (this.allAreasData[3]) {
        this.tasksStatusGeneralInfo.rejectedTasksCount = this.allAreasData[3].sumRejectedTasksCount;
      }
    }
    this.loadingTasks = false;
  }

  getWorkLocationAssignedTasksCount(workLocation: WorkLocationTasksSummary) {
    return workLocation.taskTypes.reduce((sum, t) => sum + t.assignedTasksCount, 0);
  }

  getWorkLocationAllTasksCount(workLocation: WorkLocationTasksSummary) {
    return workLocation.taskTypes.reduce((sum, t) => sum + t.allTasksCount, 0);
  }

  getTaskTypeAssignedTasksCount(workLocations: Array<WorkLocationTasksSummary>, taskTypeId: string) {
    return workLocations.reduce((sum, wl) => {
      const t = wl.taskTypes.find(tt => tt.taskTypeId === taskTypeId);
      return sum + (t ? t.assignedTasksCount : 0);
    }, 0);
  }

  getTaskTypeAllTasksCount(workLocations: Array<WorkLocationTasksSummary>, taskTypeId: string) {
    return workLocations.reduce((sum, wl) => {
      const t = wl.taskTypes.find(tt => tt.taskTypeId === taskTypeId);
      return sum + (t ? t.allTasksCount : 0);
    }, 0);
  }

  getSumAssignedTasksCount(workLocations: Array<WorkLocationTasksSummary>) {
    return workLocations.reduce((sum, wl) => sum + wl.taskTypes.reduce((s, t) => s + t.assignedTasksCount, 0), 0);
  }

  getSumAllTasksCount(workLocations: Array<WorkLocationTasksSummary>) {
    return workLocations.reduce((sum, wl) => sum + wl.taskTypes.reduce((s, t) => s + t.allTasksCount, 0), 0);
  }

  toggleAllColumns() {
    this.columnsList.options.forEach(opt => { if (!opt.disabled) opt.selected = this.allColumnsSelected; });
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

  isColumnDisabled(columnName: string): boolean { return !!this.columns.find(col => col.name === columnName && col.disabled); }
  onOpenRejectedTasks() {
    this.router.navigate(['/tasks', RoutingNodes.RejectedTasksOverview]);
  }

}

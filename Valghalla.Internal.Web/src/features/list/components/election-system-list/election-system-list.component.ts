import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { SubSink } from 'subsink';
import { MatSelectionList } from '@angular/material/list';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { TranslocoService } from '@ngneat/transloco';
import { GlobalStateService } from 'src/app/global-state.service';
import { ElectionShared } from 'src/shared/models/election/election-shared';
import { AreaShared } from 'src/shared/models/area/area-shared';
import { TeamShared } from 'src/shared/models/team/team-shared';
import { FilteredTasksHttpService } from 'src/features/tasks/services/filtered-tasks-http.service';
import { WorkLocationWithTeamIdsResponse, TaskTypeWithTeamIdsResponse } from 'src/features/tasks/models/tasks-filters-options';
import { Workbook } from 'exceljs';
import { DateTime } from 'luxon';
import { dateFormat } from 'src/shared/constants/date';

@Component({
  selector: 'election_system_list',
  templateUrl: './election-system-list.component.html',
  styleUrls: ['election-system-list.component.scss', '../../../../shared/components/table/table.component.scss'],
  providers: [FilteredTasksHttpService],
})
export class ElectionSystemList implements OnInit {
  private readonly subs = new SubSink();

  election?: ElectionShared;

  loading = true;

  loadingTasks = false;

  exporting = false;

  electionDates: Array<Date> = [];

  areas: Array<AreaShared> = [];

  teams: Array<TeamShared> = [];

  workLocations: Array<WorkLocationWithTeamIdsResponse> = [];

  taskTypes: Array<TaskTypeWithTeamIdsResponse> = [];

  selectedDates?: Array<Date> = [];

  selectedAreaIds?: Array<string> = [];

  selectedTeamIds?: Array<string> = [];

  selectedWorkLocationIds?: Array<string> = [];

  selectedTaskTypeIds?: Array<string> = [];

  selectedTaskStatusId?: number;

  dataSource: MatTableDataSource<any>;

  printDataSource: MatTableDataSource<any>;

  displayedColumns: Array<string> = ['taskTypeName', 'participantName', 'participantBirthDate', 'votingArea', 'participantUserName', 'participantCpr'];

  @ViewChild('TABLE') table: ElementRef<HTMLElement>;

  @ViewChild(MatPaginator) readonly paginator: MatPaginator;

  @ViewChild(MatSort) readonly sort: MatSort;

  @ViewChild('selectedColumns') columnsList: MatSelectionList;
  allColumnsSelected = false;

  columns = [
    {
      name: 'taskTypeName',
      key: 'list.election_system_list.labels.task_type',
      displayName: this.translocoService.translate('list.election_system_list.labels.task_type'),
      index: 1,
      disabled: false,
      isSelected: true,
    },
    {
      name: 'participantName',
      key: 'list.election_system_list.labels.full_name',
      displayName: this.translocoService.translate('list.election_system_list.labels.full_name'),
      index: 2,
      disabled: true,
      isSelected: true,
    },
    {
      name: 'participantBirthDate',
      key: 'list.election_system_list.labels.participant_birthdate',
      displayName: this.translocoService.translate('list.election_system_list.labels.participant_birthdate'),
      index: 3,
      disabled: false,
      isSelected: true,
    },
    {
      name: 'votingArea',
      key: 'list.election_system_list.labels.voting_area',
      displayName: this.translocoService.translate('list.election_system_list.labels.voting_area'),
      index: 4,
      disabled: false,
      isSelected: true,
    },
    {
      name: 'participantUserName',
      key: 'list.election_system_list.labels.participant_user_name',
      displayName: this.translocoService.translate('list.election_system_list.labels.participant_user_name'),
      index: 5,
      disabled: false,
      isSelected: true,
    },
    {
      name: 'participantCpr',
      key: 'list.election_system_list.labels.cpr_number',
      displayName: this.translocoService.translate('list.election_system_list.labels.cpr_number'),
      index: 6,
      disabled: false,
      isSelected: true,
    },
  ];

  constructor(private translocoService: TranslocoService, private globalStateService: GlobalStateService, private filteredTasksHttpService: FilteredTasksHttpService) {
    this.dataSource = new MatTableDataSource();
    this.printDataSource = new MatTableDataSource();
  }

  ngOnInit(): void {
    this.columns.forEach((col) => {
      if (this.displayedColumns.indexOf(col.name) > -1) {
        col.isSelected = true;
      }
    });
    this.subs.sink = this.globalStateService.election$.subscribe((election) => {
      this.election = election;
      if (this.election) {
        this.subs.sink = this.filteredTasksHttpService.getTasksFiltersOptions(this.election.id).subscribe((res) => {
          this.areas = res.data.areas;
          this.teams = res.data.teams;
          this.workLocations = res.data.workLocations;
          this.taskTypes = res.data.taskTypes;
          const tasksDate: Date = new Date(res.data.electionStartDate);
          while (tasksDate <= new Date(res.data.electionEndDate)) {
            this.electionDates.push(new Date(tasksDate));
            tasksDate.setDate(tasksDate.getDate() + 1);

          }
          this.loading = false;
        });
      }
    });
  }

  exportAsExcel(toCSV?: boolean) {
    this.exporting = true;

    this.subs.sink = this.filteredTasksHttpService.auditLogExport().subscribe(() => {
      const workbook = new Workbook();
      const worksheet = workbook.addWorksheet();

      worksheet.columns = this.columns.map((column) => {
        return {
          header: this.translocoService.translate(column.key),
          key: column.name,
        };
      });

      const rows = this.dataSource.data.map((item) => {
        return this.columns.reduce((obj, column) => {
          let value = item[column.name];

          if (column.name == 'taskDate') {
            value = DateTime.fromISO(value).toFormat(dateFormat);
          }
          if (column.name == 'participantBirthDate') {
            value = DateTime.fromISO(value).toFormat(dateFormat);
          }
          if (column.name == 'participantUserName') {
            value = '';
          }
          if (column.name == 'participantCpr' && value) {
            value = value.replace(/[^0-9]/g, '').padStart(10, '0').slice(0, -4) + '-' + value.slice(-4);
          }
          return { ...obj, [column.name]: value };
        }, {});
      });

      worksheet.addRows(rows);

      if (toCSV) {
        workbook.csv.writeBuffer().then((data) => {
          const blob = new Blob([data], { type: 'text/csv' });
          const url = window.URL.createObjectURL(blob);
          const anchor = document.createElement('a');
          anchor.href = url;
          anchor.download = `${DateTime.fromJSDate(new Date()).toFormat('yyyyMMdd')}-deltagere.csv`;
          anchor.click();
          window.URL.revokeObjectURL(url);

          this.exporting = false;
        });
      } else {
        workbook.xlsx.writeBuffer().then((data) => {
          const blob = new Blob([data], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
          const url = window.URL.createObjectURL(blob);
          const anchor = document.createElement('a');
          anchor.href = url;
          anchor.download = `${DateTime.fromJSDate(new Date()).toFormat('yyyyMMdd')}-deltagere.xlsx`;
          anchor.click();
          window.URL.revokeObjectURL(url);

          this.exporting = false;
        });
      }
    });
  }
  isColumnDisabled(columnName: string): boolean {
    const column = this.columns.find(col => col.name === columnName);
    return column ? column.disabled : false;
  }
  changeSelectedColumns() {
    if (this.columnsList.selectedOptions.selected.length == this.columns.length) {
      this.allColumnsSelected = true;
    } else {
      this.allColumnsSelected = false;
    }

    const selectedOptions = [];
    this.columnsList.selectedOptions.selected.forEach((selectedColumn) => {
      selectedOptions.push(selectedColumn.value);
    });

    this.displayedColumns = selectedOptions.sort((a, b) => {
      const foundA = this.columns.filter((f) => f.name == a)[0];
      const foundB = this.columns.filter((f) => f.name == b)[0];
      return this.columns.indexOf(foundA) < this.columns.indexOf(foundB) ? -1 : 1;
    });
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

  loadParticipantsTasks() {
    this.loadingTasks = true;

    const selectedOptions = [];

    selectedOptions.push('taskTypeName', 'participantName', 'participantBirthDate', 'votingArea', 'participantUserName', 'participantCpr');

    this.displayedColumns = selectedOptions.sort((a, b) => {
      const foundA = this.columns.filter((f) => f.name == a)[0];
      const foundB = this.columns.filter((f) => f.name == b)[0];
      return this.columns.indexOf(foundA) < this.columns.indexOf(foundB) ? -1 : 1;
    });

    const request: any = {
      electionId: this.election.id,
      tasksFilter: {
        teamIds: [],
        areaIds: [],
        workLocationIds: [],
        taskTypeIds: [],
        taskDates: [],
      },
    };

    if (this.selectedTeamIds && this.selectedTeamIds.length > 0) {
      request.tasksFilter.teamIds = this.selectedTeamIds;
    }
    if (this.selectedAreaIds && this.selectedAreaIds.length > 0) {
      request.tasksFilter.areaIds = this.selectedAreaIds;
    }
    if (this.selectedWorkLocationIds && this.selectedWorkLocationIds.length > 0) {
      request.tasksFilter.workLocationIds = this.selectedWorkLocationIds;
    }
    if (this.selectedTaskTypeIds && this.selectedTaskTypeIds.length > 0) {
      request.tasksFilter.taskTypeIds = this.selectedTaskTypeIds;
    }
    if (this.selectedDates && this.selectedDates.length > 0) {
      request.tasksFilter.taskDates = this.selectedDates.map((selectedDate) => {
        return new Date(selectedDate);
      });
    }
    if (this.selectedTaskStatusId > -1) {
      request.tasksFilter.taskStatus = this.selectedTaskStatusId;
    }

    this.subs.sink = this.filteredTasksHttpService.getParticipantsTasks(request).subscribe((res) => {
      this.dataSource = new MatTableDataSource<any>(res.data);
      this.dataSource.paginator = this.paginator;
      this.dataSource.sort = this.sort;

      this.printDataSource = new MatTableDataSource<any>(res.data);
      this.printDataSource.sort = this.sort;
      this.loadingTasks = false;
    });
  }
}


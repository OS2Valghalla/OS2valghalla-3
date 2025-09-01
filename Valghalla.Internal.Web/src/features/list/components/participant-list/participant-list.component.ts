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
  selector: 'app-participant-list',
  templateUrl: './participant-list.component.html',
  styleUrls: ['participant-list.component.scss', '../../../../shared/components/table/table.component.scss'],
  providers: [FilteredTasksHttpService],
})
export class ParticipantListComponent implements OnInit {
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

  displayedColumns: Array<string> = ['participantName', 'participantCpr', 'participantProtectedAddress', 'teamName', 'taskDate', 'taskTypeName'];

  @ViewChild('TABLE') table: ElementRef<HTMLElement>;

  @ViewChild(MatPaginator) readonly paginator: MatPaginator;

  @ViewChild(MatSort) readonly sort: MatSort;

  @ViewChild('selectedColumns') columnsList: MatSelectionList;
  allColumnsSelected = false;

  columns = [
    {
      name: 'participantName',
      key: 'list.participant_list.labels.full_name',
      displayName: this.translocoService.translate('list.participant_list.labels.full_name'),
      index: 1,
      disabled: true,
      isSelected: true,
    },
    {
      name: 'participantCpr',
      key: 'list.participant_list.labels.cpr_number',
      displayName: this.translocoService.translate('list.participant_list.labels.cpr_number'),
      index: 2,
      disabled: false,
      isSelected: false,
    },
    {
      name: 'participantAge',
      key: 'list.participant_list.labels.participant_age',
      displayName: this.translocoService.translate('list.participant_list.labels.participant_age'),
      index: 3,
      disabled: false,
      isSelected: false,
    },
    {
      name: 'participantPhoneNumber',
      key: 'list.participant_list.labels.participant_phone',
      displayName: this.translocoService.translate('list.participant_list.labels.participant_phone'),
      index: 4,
      disabled: false,
      isSelected: false,
    },
    {
      name: 'participantEmail',
      key: 'list.participant_list.labels.participant_email',
      displayName: this.translocoService.translate('list.participant_list.labels.participant_email'),
      index: 5,
      disabled: false,
      isSelected: false,
    },
    {
      name: 'participantAddress',
      key: 'list.participant_list.labels.participant_address',
      displayName: this.translocoService.translate('list.participant_list.labels.participant_address'),
      index: 6,
      disabled: false,
      isSelected: false,
    },
    {
      name: 'participantProtectedAddress',
      key: 'list.participant_list.labels.participant_protected_address',
      displayName: this.translocoService.translate('list.participant_list.labels.participant_protected_address'),
      index: 7,
      disabled: true,
      isSelected: true,
    },
    {
      name: 'participantSpecialDiets',
      key: 'list.participant_list.labels.participant_special_diet',
      displayName: this.translocoService.translate('list.participant_list.labels.participant_special_diet'),
      index: 8,
      disabled: false,
      isSelected: false,
    },
    {
      name: 'participantDigitalPostStatus',
      key: 'list.participant_list.labels.participant_digital_post_status',
      displayName: this.translocoService.translate('list.participant_list.labels.participant_digital_post_status'),
      index: 9,
      disabled: false,
      isSelected: false,
    },
    {
      name: 'teamName',
      key: 'list.participant_list.labels.team_association',
      displayName: this.translocoService.translate('list.participant_list.labels.team_association'),
      index: 10,
      disabled: false,
      isSelected: false,
    },
    {
      name: 'workLocation',
      key: 'list.participant_list.labels.work_location',
      displayName: this.translocoService.translate('list.participant_list.labels.work_location'),
      index: 11,
      disabled: false,
      isSelected: false,
    },
    {
      name: 'taskStatus',
      key: 'list.participant_list.labels.task_status',
      displayName: this.translocoService.translate('list.participant_list.labels.task_status'),
      index: 12,
      disabled: false,
      isSelected: false,
    },
    {
      name: 'taskDate',
      key: 'list.participant_list.labels.task_date',
      displayName: this.translocoService.translate('list.participant_list.labels.task_date'),
      index: 13,
      disabled: false,
      isSelected: false,
    },
    {
      name: 'taskTypeName',
      key: 'list.participant_list.labels.task_type',
      displayName: this.translocoService.translate('list.participant_list.labels.task_type'),
      index: 14,
      disabled: false,
      isSelected: false,
    },
    {
      name: 'areaName',
      key: 'list.participant_list.labels.task_area',
      displayName: this.translocoService.translate('list.participant_list.labels.task_area'),
      index: 15,
      disabled: false,
      isSelected: false,
    },
    {
      name: 'taskStartTime',
      key: 'list.participant_list.labels.task_start_time',
      displayName: this.translocoService.translate('list.participant_list.labels.task_start_time'),
      index: 16,
      disabled: false,
      isSelected: false,
    },
    {
      name: 'taskPayment',
      key: 'list.participant_list.labels.task_payment',
      displayName: this.translocoService.translate('list.participant_list.labels.task_payment'),
      index: 17,
      disabled: false,
      isSelected: false,
    },
    {
      name: 'receipt',
      key: 'list.participant_list.labels.receipt',
      displayName: this.translocoService.translate('list.participant_list.labels.receipt'),
      index: 18,
      disabled: false,
      isSelected: false,
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

      worksheet.columns = this.displayedColumns.map((columnName) => {
        const column = this.columns.find((i) => i.name == columnName);
        return {
          header: this.translocoService.translate(column.key),
          key: column.name,
        };
      });

      const rows = this.dataSource.data.map((item) => {
        return this.displayedColumns.reduce((obj, columnName) => {
          let value = item[columnName];

          if (columnName == 'taskDate') {
            value = DateTime.fromISO(value).toFormat(dateFormat);
          }
          if (columnName == "participant_user_name") {
            value = value ? value.substring(0, 6) : value;
          }

          return { ...obj, [columnName]: value };
        }, {});
      });

      worksheet.addRows(rows);

      if (toCSV) {
        workbook.csv.writeBuffer({ encoding: 'utf8' }).then((data) => {
          const blob = new Blob([data], { type: 'text/csv;charset=utf-8;' });
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

    if (!selectedOptions.includes('participantProtectedAddress')) {
      selectedOptions.push('participantProtectedAddress');
    }

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
    this.columnsList.selectedOptions.selected.forEach((selectedColumn) => {
      selectedOptions.push(selectedColumn.value);
    });

    if (!selectedOptions.includes('participantProtectedAddress')) {
      selectedOptions.push('participantProtectedAddress');
    }

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

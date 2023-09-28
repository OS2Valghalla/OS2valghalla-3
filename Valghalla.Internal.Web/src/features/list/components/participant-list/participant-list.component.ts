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
import {
  WorkLocationWithTeamIdsResponse,
  TaskTypeWithTeamIdsResponse,
} from 'src/features/tasks/models/tasks-filters-options';
import * as XLSX from 'xlsx';

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

  displayedColumns: Array<string> = ['participantName', 'participantCpr', 'teamName', 'taskDate', 'taskTypeName'];

  @ViewChild('TABLE') table: ElementRef;

  @ViewChild(MatPaginator) readonly paginator: MatPaginator;

  @ViewChild(MatSort) readonly sort: MatSort;

  @ViewChild('selectedColumns') columnsList: MatSelectionList;
  allColumnsSelected = false;

  columns = [
    {
      name: 'participantName',
      displayName: this.translocoService.translate('list.participant_list.labels.full_name'),
      index: 1,
      disabled: true,
      isSelected: true,
    },
    {
      name: 'participantCpr',
      displayName: this.translocoService.translate('list.participant_list.labels.cpr_number'),
      index: 2,
    },
    {
      name: 'participantAge',
      displayName: this.translocoService.translate('list.participant_list.labels.participant_age'),
      index: 3,
    },
    {
      name: 'participantPhoneNumber',
      displayName: this.translocoService.translate('list.participant_list.labels.participant_phone'),
      index: 4,
    },
    {
      name: 'participantEmail',
      displayName: this.translocoService.translate('list.participant_list.labels.participant_email'),
      index: 5,
    },
    {
      name: 'participantAddress',
      displayName: this.translocoService.translate('list.participant_list.labels.participant_address'),
      index: 6,
    },
    {
      name: 'participantSpecialDiets',
      displayName: this.translocoService.translate('list.participant_list.labels.participant_special_diet'),
      index: 7,
    },
    {
      name: 'teamName',
      displayName: this.translocoService.translate('list.participant_list.labels.team_association'),
      index: 8,
    },
    {
      name: 'taskStatus',
      displayName: this.translocoService.translate('list.participant_list.labels.task_status'),
      index: 9,
    },
    {
      name: 'taskDate',
      displayName: this.translocoService.translate('list.participant_list.labels.task_date'),
      index: 10,
    },
    {
      name: 'taskTypeName',
      displayName: this.translocoService.translate('list.participant_list.labels.task_type'),
      index: 11,
    },
    {
      name: 'areaName',
      displayName: this.translocoService.translate('list.participant_list.labels.task_area'),
      index: 12,
    },
    {
      name: 'taskStartTime',
      displayName: this.translocoService.translate('list.participant_list.labels.task_start_time'),
      index: 13,
    },
    {
      name: 'taskPayment',
      displayName: this.translocoService.translate('list.participant_list.labels.task_payment'),
      index: 14,
    },
    {
      name: 'receipt',
      displayName: this.translocoService.translate('list.participant_list.labels.receipt'),
      index: 15,
    },
  ];

  constructor(
    private translocoService: TranslocoService,
    private globalStateService: GlobalStateService,
    private filteredTasksHttpService: FilteredTasksHttpService,
  ) {
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
          var tasksDate: Date = new Date(res.data.electionStartDate);
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
    this.subs.sink = this.filteredTasksHttpService.auditLogExport().subscribe(() => {
      this.exporting = true;
      const ws: XLSX.WorkSheet = XLSX.utils.table_to_sheet(this.table.nativeElement);
      const wb: XLSX.WorkBook = XLSX.utils.book_new();
      XLSX.utils.book_append_sheet(wb, ws, 'Participants');
      toCSV ? XLSX.writeFile(wb, 'Participants.csv', { bookType: 'csv' }) : XLSX.writeFile(wb, 'Participants.xlsx');
      this.exporting = false;
    });
  }

  changeSelectedColumns() {
    if (this.columnsList.selectedOptions.selected.length == this.columns.length) {
      this.allColumnsSelected = true;
    } else {
      this.allColumnsSelected = false;
    }

    var selectedOptions = [];
    this.columnsList.selectedOptions.selected.forEach((selectedColumn) => {
      selectedOptions.push(selectedColumn.value);
    });

    this.displayedColumns = selectedOptions.sort((a, b) => {
      var foundA = this.columns.filter((f) => f.name == a)[0];
      var foundB = this.columns.filter((f) => f.name == b)[0];
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

    var selectedOptions = [];
    this.columnsList.selectedOptions.selected.forEach((selectedColumn) => {
      selectedOptions.push(selectedColumn.value);
    });

    this.displayedColumns = selectedOptions.sort((a, b) => {
      var foundA = this.columns.filter((f) => f.name == a)[0];
      var foundB = this.columns.filter((f) => f.name == b)[0];
      return this.columns.indexOf(foundA) < this.columns.indexOf(foundB) ? -1 : 1;
    });

    var request: any = {
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

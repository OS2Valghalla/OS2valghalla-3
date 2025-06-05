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
import { Router } from '@angular/router';

export interface RejectedTasksDetailsReponse {
  participantName: string;
  paskTypeName: string;
  taskDate: string;
  AreaName: string;
  TeamName: string;
  workLocationName: string;
}

@Component({
  selector: 'app-rejected-tasks-overview',
  templateUrl: './rejected-tasks-overview.component.html',
  styleUrls: ['../../../../shared/components/table/table.component.scss', 'rejected-tasks-overview.component.scss'],
  providers: [AreaSharedHttpService, AreaTasksHttpService],
})
export class RejectedTasksOverviewComponent implements AfterViewInit {

  private readonly subs = new SubSink();

  election?: ElectionShared;

  rejectedTasksDetails: RejectedTasksDetailsReponse[] = [];
  displayedColumns: string[] = [
    'status',
    'participantName',
    'taskTypeName',
    'taskDate',
    'areaName',
    'teamName',
    'actions',
  ];

  constructor(private globalStateService: GlobalStateService, private areaTasksHttpService: AreaTasksHttpService, private router: Router) { }

  ngAfterViewInit(): void {
    this.subs.sink = this.globalStateService.election$.subscribe((election) => {
      this.election = election;
    });
    if (this.election) {
      this.areaTasksHttpService.getRejectedTasksInfo(this.election.id).subscribe((res) => {
        this.rejectedTasksDetails = res.data;
      });
    }

  }

  getUniqueWorkLocations(): string[] {
    const names = this.rejectedTasksDetails.map(t => t.workLocationName).filter(Boolean);
    return Array.from(new Set(names));
  }

  getTasksByWorkLocation(workLocationName: string) {
    return this.rejectedTasksDetails.filter(t => t.workLocationName === workLocationName);
  }
}

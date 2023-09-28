import { Component, Input, OnDestroy, ViewChild } from '@angular/core';
import { SubSink } from 'subsink';
import { TranslocoService } from '@ngneat/transloco';
import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { ElectionShared } from 'src/shared/models/election/election-shared';
import { GlobalStateService } from 'src/app/global-state.service';
import { Router } from '@angular/router';
import { RoutingNodes } from 'src/shared/constants/routing-nodes';
import { MatDialog } from '@angular/material/dialog';
import { ConfirmationDialogComponent } from 'src/shared/components/confirmation-dialog/confirmation-dialog.component';
import { ElectionHttpService } from 'src/features/administration/election/services/election-http.service';
import { SELECTED_ELECTION_TO_WORK_ON } from 'src/shared/constants/election';
import { ParticipantHttpService } from '../../../services/participant-http.service';
import { ParticipantTask } from '../../../models/participant-task';

@Component({
  selector: 'app-participant-task-status-listing',
  templateUrl: './participant-task-status-listing.component.html',
  styleUrls: ['./participant-task-status-listing.component.scss'],
  providers: [ElectionHttpService, ParticipantHttpService]
})
export class ParticipantTaskStatusListingComponent implements OnDestroy {
  private readonly subs = new SubSink();

  loadingTasks = true;

  election?: ElectionShared;

  @Input() participantId: string;

  @ViewChild(MatPaginator) readonly paginator: MatPaginator;

  @ViewChild(MatSort) readonly sort: MatSort;

  dataSource: MatTableDataSource<ParticipantTask>;

  constructor(
    private router: Router,
    private dialog: MatDialog,
    private globalStateService: GlobalStateService,
    private translocoService: TranslocoService,
    private electionHttpService: ElectionHttpService,
    private participantHttpService: ParticipantHttpService) {}

  ngOnDestroy(): void {
    this.subs.unsubscribe();
  }

  ngAfterViewInit() {
    this.subs.sink = this.globalStateService.election$.subscribe((election) => {
      this.election = election;
      this.subs.sink = this.participantHttpService.getParticipantTasks(this.participantId).subscribe((res) => {
        this.dataSource = new MatTableDataSource<ParticipantTask>(res.data);
        this.dataSource.paginator = this.paginator;
        this.dataSource.sort = this.sort;
        this.loadingTasks = false;
      });
    });
  }
  
  openTaskDistribution(row: ParticipantTask) {
    if (row.electionId != this.election.id) {
      const dialogRef = this.dialog.open(ConfirmationDialogComponent, {
        minWidth: 400,
        data: {
          title: "participant.task_status.change_election_confirmation.title",
          content: this.translocoService.translate('participant.task_status.change_election_confirmation.content') + ' ' + row.electionName,
        },
      });
  
      this.subs.sink = dialogRef.afterClosed().subscribe((result) => {
        if (result === true) {
          localStorage.setItem(SELECTED_ELECTION_TO_WORK_ON, row.electionId);
          this.subs.sink = this.electionHttpService.getElection(row.electionId).subscribe((res) => {
            this.globalStateService.changeElectionToWorkOn({
              id: res.data.id,
              title: res.data.title,
              active: res.data.active,
              electionDate: res.data.electionDate
            });
            this.router.navigate([RoutingNodes.TasksOnWorkLocation, row.workLocationId]);
          });          
        }
      });
    }
    else {
      this.router.navigate([RoutingNodes.TasksOnWorkLocation, row.workLocationId]);
    }
  }
}

import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { ElectionHttpService } from 'src/features/administration/election/services/election-http.service';
import { SubSink } from 'subsink';
import { SELECTED_ELECTION_TO_WORK_ON } from 'src/shared/constants/election';
import { GlobalStateService } from 'src/app/global-state.service';
import { RoutingNodes } from 'src/shared/constants/routing-nodes';
import { QueryEvent, QueryForm, QueryFormEvent } from 'src/shared/query-engine/models/query-form';
import { ElectionShared } from 'src/shared/models/election/election-shared';
import { TableComponent } from 'src/shared/components/table/table.component';

@Component({
  selector: 'app-admin-election',
  templateUrl: './election.component.html',
  styleUrls: ['./election.component.scss'],
  providers: [ElectionHttpService],
})
export class ElectionComponent implements OnInit, OnDestroy {
  private readonly subs = new SubSink();

  loading = true;
  data: Array<ElectionShared> = [];

  @ViewChild(TableComponent) private readonly table: TableComponent<ElectionShared>;

  constructor(
    private router: Router,
    private globalStateService: GlobalStateService,
    private readonly electionHttpService: ElectionHttpService,
  ) {}

  ngOnInit(): void {
  }

  ngOnDestroy() {
    this.subs.unsubscribe();
  }

  onQuery(event: QueryEvent<QueryForm>) {
    if (!event.query.order) {
      event.query.order = {
        name: 'title',
        descending: false,
      };
    }
    event.execute('administration/election/queryelectionlisting', event.query);
  }

  onQueryForm(event: QueryFormEvent<void>) {
    event.execute('administration/election/getelectionlistingqueryform');
  }

  openAddElection() {
    this.router.navigate([RoutingNodes.Administration, RoutingNodes.Election, RoutingNodes.Link_Create]);
  }

  editElectionDetails = (election: ElectionShared) => {
    this.router.navigate([RoutingNodes.Administration, RoutingNodes.Election, RoutingNodes.Link_Edit, election.id]);
  };

  editCommunicationConfigurations = (election: ElectionShared) => {
    this.router.navigate([
      RoutingNodes.Administration,
      RoutingNodes.Election,
      RoutingNodes.Link_Edit + '-communication-configuration',
      election.id,
    ]);
  };

  duplicateElection = (election: ElectionShared) => {
    this.router.navigate([RoutingNodes.Administration, RoutingNodes.Election, RoutingNodes.Link_Duplicate, election.id]);
  };

  deleteElection = (election: ElectionShared) => {
    this.subs.sink = this.electionHttpService.deleteElection(election.id).subscribe((res) => {
      if (res.isSuccess) {
        this.table.refresh();
      }
    });
  };

  checkIfShowDeleteElectionButton = (election: ElectionShared) => {
    return (election.id != localStorage.getItem(SELECTED_ELECTION_TO_WORK_ON) && !election.active);
  }

  checkIfShowCannotDeleteActiveElectionButton = (election: ElectionShared) => {
    return (election.id != localStorage.getItem(SELECTED_ELECTION_TO_WORK_ON) && election.active);
  }
  
  checkIfShowCannotDeleteSelectedElectionButton = (election: ElectionShared) => {
    return election.id == localStorage.getItem(SELECTED_ELECTION_TO_WORK_ON);
  }

  activateElection(election: ElectionShared) {
    this.electionHttpService.activateElection(election.id).subscribe((res) => {
      if (res.isSuccess) {
        this.table.refresh();
      }
    });
  }

  deactivateElection(election: ElectionShared) {
    this.electionHttpService.deactivateElection(election.id).subscribe((res) => {
      if (res.isSuccess) {
        this.table.refresh();
      }
    });
  }
}

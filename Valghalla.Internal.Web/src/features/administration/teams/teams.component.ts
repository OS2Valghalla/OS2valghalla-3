import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { TeamHttpService } from './services/teams-http.service';
import { TableComponent } from 'src/shared/components/table/table.component';
import { SubSink } from 'subsink';
import { Team } from './models/team';
import { QueryEvent, QueryForm, QueryFormEvent } from 'src/shared/query-engine/models/query-form';
import { RoutingNodes } from 'src/shared/constants/routing-nodes';
import { MatDialog } from '@angular/material/dialog';
import { CopyInviteLinkDialogComponent } from './components/copy-invite-link-dialog/copy-invite-link-dialog.component';

interface TeamsQueryForm extends QueryForm {}

@Component({
  selector: 'app-admin-teams',
  templateUrl: './teams.component.html',
  providers: [TeamHttpService],
})
export class TeamsComponent implements OnInit, OnDestroy {
  private readonly subs = new SubSink();

  loading = true;

  data: Array<Team> = [];

  @ViewChild('tableTeams') private readonly tableTeams: TableComponent<Team>;

  constructor(private router: Router, private teamHttpService: TeamHttpService, private dialog: MatDialog) {}

  ngOnInit(): void {}

  ngOnDestroy() {
    this.subs.unsubscribe();
  }

  onQuery(event: QueryEvent<TeamsQueryForm>) {
    if (!event.query.order) {
      event.query.order = {
        name: 'name',
        descending: false,
      };
    }
    event.execute('administration/team/queryteamlisting', event.query);
  }

  onQueryForm(event: QueryFormEvent<void>) {
    event.execute('administration/team/getteamlistingqueryform');
  }

  deleteTeam = (event: Team) => {
    this.subs.sink = this.teamHttpService.deleteTeam(event.id).subscribe((res) => {
      if (res.isSuccess) {
        this.tableTeams.refresh();
      }
    });
  };

  openAddTeam() {
    this.router.navigate([RoutingNodes.Administration, RoutingNodes.Teams, RoutingNodes.Link_Create]);
  }

  openCopyInviteLink = (event: Team) => {
    this.subs.sink = this.teamHttpService.createTeamLink(event.id).subscribe((res) => {
      if (res.isSuccess) {
        this.dialog.open(CopyInviteLinkDialogComponent, { data: { name: event.name, link: res.data } });
      }
    });
  };

  openEditTeam = (event: Team) => {
    this.router.navigate([RoutingNodes.Administration, RoutingNodes.Teams, RoutingNodes.Link_Edit, event.id]);
  };
}

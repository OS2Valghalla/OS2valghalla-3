import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { TeamHttpService } from './services/teams-http.service';
import { TableComponent } from 'src/shared/components/table/table.component';
import { TableEditRowEvent } from 'src/shared/models/ux/table';
import { SubSink } from 'subsink';
import { Team } from './models/team';
import { QueryEvent, QueryForm, QueryFormEvent } from 'src/shared/query-engine/models/query-form';
import { RoutingNodes } from 'src/shared/constants/routing-nodes';

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

  constructor(private router: Router, private teamHttpService: TeamHttpService) {}

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

  deleteTeam(event: TableEditRowEvent<Team>) {
    this.subs.sink = this.teamHttpService.deleteTeam(event.row.id).subscribe((res) => {
      if (res.isSuccess) {
        this.tableTeams.refresh();
      }
    });
  }

  openAddTeam() {
    this.router.navigate([RoutingNodes.Administration, RoutingNodes.Teams, RoutingNodes.Link_Create]);
  }

  openEditTeam(event: TableEditRowEvent<Team>) {
    this.router.navigate([RoutingNodes.Administration, RoutingNodes.Teams, RoutingNodes.Link_Edit, event.row.id]);
  }
}

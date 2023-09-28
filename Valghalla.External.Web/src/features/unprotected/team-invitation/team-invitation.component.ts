import { Component, AfterViewInit } from '@angular/core';
import { SubSink } from 'subsink';
import { ActivatedRoute, Router } from '@angular/router';
import { RoutingNodes } from 'src/shared/constants/routing-nodes';
import { Team } from '../../my-team/models/team';
import { UnprotectedTeamHttpService } from '../../my-team/services/unprotected-team-http.service';

@Component({
  selector: 'app-team-invitation',
  templateUrl: './team-invitation.component.html',
  providers: [UnprotectedTeamHttpService],
})
export class TeamInvitationComponent implements AfterViewInit {
  private readonly subs = new SubSink();

  loading = true;

  hashValue: string;

  team: Team;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private unprotectedTeamHttpService: UnprotectedTeamHttpService,
  ) {}

  ngAfterViewInit() {
    this.hashValue = this.route.snapshot.queryParamMap.get(RoutingNodes.Id);
    this.subs.sink = this.unprotectedTeamHttpService.getTeamByTeamLink(this.hashValue).subscribe((res) => {
      if (res.data) {
        this.team = res.data;
      }
      this.loading = false;
    });
  }

  goToCreateUser() {
    this.router.navigate([RoutingNodes.Registration, RoutingNodes.TeamRegistration, this.hashValue]);
  }
}

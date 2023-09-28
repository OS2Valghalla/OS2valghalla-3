import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { SubSink } from 'subsink';
import { TeamSharedHttpService } from 'src/shared/services/team-shared-http.service';
import { TeamShared } from 'src/shared/models/team/team-shared';
import { combineLatest } from 'rxjs';
import { Router } from '@angular/router';
import { RoutingNodes } from 'src/shared/constants/routing-nodes';
import { ParticipantHttpService } from 'src/features/participant/services/participant-http.service';

@Component({
  selector: 'app-team-responsible-rights',
  templateUrl: './team-responsible-rights.component.html',
  providers: [ParticipantHttpService],
})
export class TeamResponsibleRightsComponent implements OnInit, OnDestroy {
  private readonly subs = new SubSink();

  @Input() participantId: string;

  loading: boolean = true;

  teams: TeamShared[] = [];
  teamResponsibleRights: string[] = [];

  constructor(
    private readonly router: Router,
    private readonly teamSharedHttpService: TeamSharedHttpService,
    private readonly participantHttpService: ParticipantHttpService,
  ) {}

  ngOnInit(): void {
    this.subs.sink = combineLatest({
      teamResponsibleRights: this.participantHttpService.getTeamResponsibleRights(this.participantId),
      teams: this.teamSharedHttpService.getTeams(),
    }).subscribe((d) => {
      this.loading = false;
      this.teams = d.teams.data;
      this.teamResponsibleRights = d.teamResponsibleRights.data;
    });
  }

  ngOnDestroy(): void {
    this.subs.unsubscribe();
  }

  openEditTeam(teamId: string) {
    this.router.navigate([RoutingNodes.Administration, RoutingNodes.Teams, RoutingNodes.Link_Edit, teamId]);
  }

  renderTeamName(teamId: string) {
    const value = this.teams.find((i) => i.id == teamId);
    return value?.name ?? teamId;
  }
}

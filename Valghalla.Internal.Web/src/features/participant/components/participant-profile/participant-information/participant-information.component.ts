import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { SubSink } from 'subsink';
import { ParticipantDetails } from '../../../models/participant-details';
import { TeamSharedHttpService } from 'src/shared/services/team-shared-http.service';
import { TeamShared } from 'src/shared/models/team/team-shared';
import { combineLatest } from 'rxjs';
import { SpecialDietSharedHttpService } from 'src/shared/services/special-diet-shared-http.service';
import { SpecialDietShared } from 'src/shared/models/special-diet/special-diet-shared';
import { Router } from '@angular/router';
import { RoutingNodes } from 'src/shared/constants/routing-nodes';

@Component({
  selector: 'app-participant-information',
  templateUrl: './participant-information.component.html',
})
export class ParticipantInformationComponent implements OnInit, OnDestroy {
  private readonly subs = new SubSink();

  @Input() participant: ParticipantDetails;

  loading: boolean = true;

  teams: TeamShared[] = [];
  specialDiets: SpecialDietShared[] = [];

  constructor(
    private readonly router: Router,
    private readonly teamSharedHttpService: TeamSharedHttpService,
    private readonly specialDietSharedHttpService: SpecialDietSharedHttpService,
  ) {}

  ngOnInit(): void {
    this.subs.sink = combineLatest({
      teams: this.teamSharedHttpService.getTeams(),
      specialDiets: this.specialDietSharedHttpService.getSpecialDiets(),
    }).subscribe((d) => {
      this.loading = false;
      this.teams = d.teams.data;
      this.specialDiets = d.specialDiets.data;
    });
  }

  ngOnDestroy(): void {
    this.subs.unsubscribe();
  }

  openEditParticipant() {
    if (!this.participant) return;
    this.router.navigate([RoutingNodes.Participant, RoutingNodes.Link_Edit, this.participant.id]);
  }

  renderTeamNames(teamIds: string[]) {
    if (!teamIds) return undefined;

    const names = teamIds.map((id) => {
      const value = this.teams.find((i) => i.id == id);
      return value?.name ?? id;
    });

    return names.join(', ');
  }

  renderSpecialDietTitles(specialDietIds: string[]) {
    if (!specialDietIds) return undefined;

    const names = specialDietIds.map((id) => {
      const value = this.specialDiets.find((i) => i.id == id);
      return value?.title ?? id;
    });

    return names.join(', ');
  }
}

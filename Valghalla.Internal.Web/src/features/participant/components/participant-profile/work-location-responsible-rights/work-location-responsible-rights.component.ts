import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { SubSink } from 'subsink';
import { combineLatest } from 'rxjs';
import { Router } from '@angular/router';
import { RoutingNodes } from 'src/shared/constants/routing-nodes';
import { ParticipantHttpService } from 'src/features/participant/services/participant-http.service';
import { WorkLocationShared } from 'src/shared/models/work-location/work-location-shared';
import { WorkLocationSharedHttpService } from 'src/shared/services/work-location-shared-http.service';

@Component({
  selector: 'app-work-location-responsible-rights',
  templateUrl: './work-location-responsible-rights.component.html',
  providers: [ParticipantHttpService],
})
export class WorkLocationResponsibleRightsComponent implements OnInit, OnDestroy {
  private readonly subs = new SubSink();

  @Input() participantId: string;

  loading: boolean = true;

  workLocations: WorkLocationShared[] = [];
  workLocationResponsibleRights: string[] = [];

  constructor(
    private readonly router: Router,
    private readonly workLocationSharedHttpService: WorkLocationSharedHttpService,
    private readonly participantHttpService: ParticipantHttpService,
  ) {}

  ngOnInit(): void {
    this.subs.sink = combineLatest({
      workLocationResponsibleRights: this.participantHttpService.getWorkLocationResponsibleRights(this.participantId),
      workLocations: this.workLocationSharedHttpService.getWorkLocations(),
    }).subscribe((d) => {
      this.loading = false;
      this.workLocations = d.workLocations.data;
      this.workLocationResponsibleRights = d.workLocationResponsibleRights.data;
    });
  }

  ngOnDestroy(): void {
    this.subs.unsubscribe();
  }

  openEditWorkLocation(workLocationId: string) {
    this.router.navigate([
      RoutingNodes.Administration,
      RoutingNodes.WorkLocation,
      RoutingNodes.Link_Edit,
      workLocationId,
    ]);
  }

  renderWorkLocationTitle(workLocationId: string) {
    const value = this.workLocations.find((i) => i.id == workLocationId);
    return value?.title ?? workLocationId;
  }
}

import { Component, OnDestroy, OnInit } from '@angular/core';
import { SubSink } from 'subsink';
import { ActivatedRoute, Router } from '@angular/router';
import { RoutingNodes } from 'src/shared/constants/routing-nodes';
import { ParticipantHttpService } from 'src/features/participant/services/participant-http.service';
import { ParticipantDetails } from '../../models/participant-details';
import { catchError, throwError } from 'rxjs';

@Component({
  selector: 'app-participant-profile',
  templateUrl: './participant-profile.component.html',
  providers: [ParticipantHttpService],
})
export class ParticipantProfileComponent implements OnInit, OnDestroy {
  private readonly subs = new SubSink();

  participantId: string;
  participant: ParticipantDetails;
  deleting: boolean = false;

  constructor(
    private readonly route: ActivatedRoute,
    private readonly router: Router,
    private readonly participantHttpService: ParticipantHttpService,
  ) {}

  ngOnInit(): void {
    this.participantId = this.route.snapshot.paramMap.get(RoutingNodes.Id);

    this.subs.sink = this.participantHttpService.getParticipantDetails(this.participantId).subscribe((res) => {
      this.participant = res.data;
    });
  }

  ngOnDestroy(): void {
    this.subs.unsubscribe();
  }

  deleteParticipant() {
    this.subs.sink = this.participantHttpService
      .deleteParticipant(this.participantId)
      .pipe(
        catchError((error) => {
          this.deleting = false;
          return throwError(() => error);
        }),
      )
      .subscribe((res) => {
        this.deleting = false;
        if (res.isSuccess) {
          this.router.navigate([RoutingNodes.Participant]);
        }
      });
  }
}

import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { SubSink } from 'subsink';

@Component({
  selector: 'app-participant-communication-log-listing',
  templateUrl: './participant-communication-log-listing.component.html',
})
export class ParticipantCommunicationLogListingComponent implements OnInit, OnDestroy {
  private readonly subs = new SubSink();

  @Input() participantId: string;

  ngOnInit(): void {}

  ngOnDestroy(): void {
    this.subs.unsubscribe();
  }
}

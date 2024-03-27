import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { CommunicationLogListingItem } from 'src/features/communication/communication-log/models/communication-log-listing-item';
import { RoutingNodes } from 'src/shared/constants/routing-nodes';
import { MultipleSelectionFilterValue } from 'src/shared/query-engine/models/multiple-selection-filter-value';
import { QueryEvent, QueryForm, QueryFormEvent } from 'src/shared/query-engine/models/query-form';
import { SubSink } from 'subsink';

interface CommunicationLogListingQueryForm extends QueryForm {
  status?: MultipleSelectionFilterValue<number>;
  participantId: string;
}

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

  loading = true;
  data: Array<CommunicationLogListingItem> = [];
  queryTyping: CommunicationLogListingQueryForm;

  constructor(private readonly router: Router) {}

  onQuery(event: QueryEvent<CommunicationLogListingQueryForm>) {
    event.query.participantId = this.participantId;
    event.execute('communication/log/queryloglisting', event.query);
  }

  onQueryForm(event: QueryFormEvent<void>) {
    event.execute('communication/log/getloglistingqueryform', {} as any);
  }

  getRowNgClass(row: CommunicationLogListingItem) {
    return {
      'communication-log-error': row.status?.value == 3,
    };
  }

  openParticipantProfile(row: CommunicationLogListingItem) {
    this.router.navigate([RoutingNodes.Participant, RoutingNodes.Profile, row.participantId]);
  }

  openDetails(row: CommunicationLogListingItem) {
    this.router.navigate([RoutingNodes.Communication, RoutingNodes.CommunicationLogs, RoutingNodes.Details, row.id]);
  }
}

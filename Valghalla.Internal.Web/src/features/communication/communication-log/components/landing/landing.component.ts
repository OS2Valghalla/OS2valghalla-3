import { Component, OnDestroy, ViewEncapsulation } from '@angular/core';
import { SubSink } from 'subsink';
import { QueryEvent, QueryForm, QueryFormEvent } from 'src/shared/query-engine/models/query-form';
import { Router } from '@angular/router';
import { RoutingNodes } from 'src/shared/constants/routing-nodes';
import { MultipleSelectionFilterValue } from 'src/shared/query-engine/models/multiple-selection-filter-value';
import { CommunicationLogListingItem } from '../../models/communication-log-listing-item';

interface CommunicationLogListingQueryForm extends QueryForm {
  status?: MultipleSelectionFilterValue<number>;
}

@Component({
  selector: 'app-communication-log-landing',
  templateUrl: './landing.component.html',
  styleUrls: ['./landing.component.scss'],
  encapsulation: ViewEncapsulation.None,
})
export class CommunicationLogLandingComponent implements OnDestroy {
  private readonly subs = new SubSink();

  loading = true;
  data: Array<CommunicationLogListingItem> = [];
  queryTyping: CommunicationLogListingQueryForm;

  constructor(private readonly router: Router) {}

  ngOnDestroy() {
    this.subs.unsubscribe();
  }

  onQuery(event: QueryEvent<CommunicationLogListingQueryForm>) {
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

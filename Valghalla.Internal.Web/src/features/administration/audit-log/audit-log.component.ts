import { Component } from '@angular/core';
import { AuditLogListingItem } from './models/audit-log-listing-item';
import { QueryEvent, QueryForm, QueryFormEvent, QueryPrepareEvent } from 'src/shared/query-engine/models/query-form';
import { MultipleSelectionFilterValue } from 'src/shared/query-engine/models/multiple-selection-filter-value';
import { AuditLogEventType } from './models/audit-log-event-type';
import { AuditLogEventTable } from './models/audit-log-event-table';
import { Router } from '@angular/router';
import { RoutingNodes } from 'src/shared/constants/routing-nodes';
import { TranslocoService } from '@ngneat/transloco';

interface AuditLogListingQueryForm extends QueryForm {
  eventType?: MultipleSelectionFilterValue<string>;
  eventTable?: MultipleSelectionFilterValue<string>;
}

@Component({
  selector: 'app-audit-log-landing',
  templateUrl: './audit-log.component.html',
})
export class AuditLogLandingComponent {
  data: Array<AuditLogListingItem> = [];
  queryTyping: AuditLogListingQueryForm;

  constructor(private readonly router: Router, private readonly translocoService: TranslocoService) {}

  onQuery(event: QueryEvent<AuditLogListingQueryForm>) {
    event.execute('administration/auditlog/queryauditloglisting', event.query);
  }

  onPrepareQuery(event: QueryPrepareEvent<AuditLogListingQueryForm>) {
    if (!event.isInitialQuery) return;

    const text = this.translocoService.translate('administration.audit_log.event_table.participant');

    event.query.eventTable = {
      values: [AuditLogEventTable.Participant],
    };

    event.updateQuery(
      'eventTable',
      { values: [AuditLogEventTable.Participant] },
      'administration.audit_log.audit_log_listing.columns.event_table',
      [text],
      true,
    );
  }

  onQueryForm(event: QueryFormEvent<void>) {
    event.execute('administration/auditlog/getauditloglistingqueryform', {} as any);
  }

  openParticipantProfile(participantId: string) {
    this.router.navigate([RoutingNodes.Participant, RoutingNodes.Profile, participantId]);
  }

  renderEventType(value: string) {
    if (value == AuditLogEventType.Create) return 'administration.audit_log.event_type.create';
    if (value == AuditLogEventType.View) return 'administration.audit_log.event_type.view';
    if (value == AuditLogEventType.Edit) return 'administration.audit_log.event_type.edit';
    if (value == AuditLogEventType.Delete) return 'administration.audit_log.event_type.delete';
    if (value == AuditLogEventType.LookUpCpr) return 'administration.audit_log.event_type.look_up_cpr';
    if (value == AuditLogEventType.Generate) return 'administration.audit_log.event_type.generate';
    if (value == AuditLogEventType.Export) return 'administration.audit_log.event_type.export';
    if (value == AuditLogEventType.Request) return 'administration.audit_log.event_type.request';

    return value;
  }

  renderEventTable(value: string) {
    if (value == AuditLogEventTable.Participant) return 'administration.audit_log.event_table.participant';
    if (value == AuditLogEventTable.List) return 'administration.audit_log.event_table.list';
    if (value == AuditLogEventTable.TeamResponsible) return 'administration.audit_log.event_table.team_responsible';
    if (value == AuditLogEventTable.WorkLocationResponsible)
      return 'administration.audit_log.event_table.work_location_responsible';
    if (value == AuditLogEventTable.API) return 'administration.audit_log.event_table.api';
    if (value == AuditLogEventTable.Others) return 'administration.audit_log.event_table.others';

    return value;
  }
}

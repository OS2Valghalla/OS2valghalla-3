import { Component, Input, OnDestroy, ViewChild } from '@angular/core';
import { SubSink } from 'subsink';
import { TableComponent } from 'src/shared/components/table/table.component';
import { QueryEvent, QueryForm } from 'src/shared/query-engine/models/query-form';
import { TableEditRowEvent } from 'src/shared/models/ux/table';
import { ParticipantEventLogListingItem } from 'src/features/participant/models/participant-event-log-listing-item';
import { MatDialog } from '@angular/material/dialog';
import { ParticipantEventLogItemComponent } from './participant-event-log-item.component';
import { ConfirmationDialogComponent } from 'src/shared/components/confirmation-dialog/confirmation-dialog.component';
import { ParticipantHttpService } from 'src/features/participant/services/participant-http.service';

interface ParticipantEventLogListingQueryForm extends QueryForm {
  participantId: string;
}

@Component({
  selector: 'app-participant-event-log-listing',
  templateUrl: './participant-event-log-listing.component.html',
  providers: [ParticipantHttpService],
})
export class ParticipantEventLogListingComponent implements OnDestroy {
  private readonly subs = new SubSink();

  @Input() participantId: string;

  data: Array<ParticipantEventLogListingItem> = [];
  queryTyping: ParticipantEventLogListingQueryForm;

  @ViewChild(TableComponent) private readonly table: TableComponent<ParticipantEventLogListingItem>;

  constructor(private readonly dialog: MatDialog, private readonly participantHttpService: ParticipantHttpService) {}

  ngOnDestroy() {
    this.subs.unsubscribe();
  }

  onQuery(event: QueryEvent<ParticipantEventLogListingQueryForm>) {
    event.query.participantId = this.participantId;
    event.execute('participant/queryparticipanteventloglisting', event.query);
  }

  openAddParticipantEventLog() {
    this.subs.sink = this.dialog
      .open(ParticipantEventLogItemComponent, {
        data: { participantId: this.participantId },
      })
      .afterClosed()
      .subscribe((res) => {
        if (res == 'success') {
          this.table.refresh();
        }
      });
  }

  openEditParticipantEventLog(event: TableEditRowEvent<ParticipantEventLogListingItem>) {
    this.subs.sink = this.dialog
      .open(ParticipantEventLogItemComponent, {
        data: { participantId: this.participantId, item: event.row },
      })
      .afterClosed()
      .subscribe((res) => {
        if (res == 'success') {
          this.table.refresh();
        }
      });
  }

  deleteParticipantEventLog(event: TableEditRowEvent<ParticipantEventLogListingItem>) {
    this.subs.sink = this.subs.sink = this.dialog
      .open(ConfirmationDialogComponent)
      .afterClosed()
      .subscribe((result) => {
        if (result === true) {
          this.subs.sink = this.participantHttpService.deleteParticipantEventLog(event.row.id).subscribe((result) => {
            this.table.refresh();
          });
        }
      });
  }
}

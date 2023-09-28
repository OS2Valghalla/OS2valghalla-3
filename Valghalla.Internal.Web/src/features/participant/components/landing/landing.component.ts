import { Component, OnDestroy, ViewChild } from '@angular/core';
import { SubSink } from 'subsink';
import { ParticipantListingItem } from '../../models/participant-listing-item';
import { Router } from '@angular/router';
import { QueryEvent, QueryForm, QueryFormEvent } from 'src/shared/query-engine/models/query-form';
import { MultipleSelectionFilterValue } from 'src/shared/query-engine/models/multiple-selection-filter-value';
import { BooleanFilterValue } from 'src/shared/query-engine/models/boolean-filter-value';
import { RoutingNodes } from 'src/shared/constants/routing-nodes';
import { TableEditRowEvent, TableSelectRowsEvent } from 'src/shared/models/ux/table';
import { TeamSharedHttpService } from 'src/shared/services/team-shared-http.service';
import { catchError, combineLatest, throwError } from 'rxjs';
import { TeamShared } from 'src/shared/models/team/team-shared';
import { TaskTypeSharedHttpService } from 'src/shared/services/task-type-shared-http.service';
import { TaskTypeShared } from 'src/shared/models/task-type/task-type-shared';
import { DateTimeFilterValue } from 'src/shared/query-engine/models/date-time-filter-value';
import { ParticipantHttpService } from '../../services/participant-http.service';
import { TableComponent } from 'src/shared/components/table/table.component';

interface ParticipantListingQueryForm extends QueryForm {
  birthdate?: DateTimeFilterValue;
  teams?: MultipleSelectionFilterValue<string>;
  digitalPost?: BooleanFilterValue;
  assignedTask?: BooleanFilterValue;
  taskTypes?: MultipleSelectionFilterValue<string>;
}

@Component({
  selector: 'app-participant-landing',
  templateUrl: './landing.component.html',
  providers: [ParticipantHttpService],
})
export class ParticipantLandingComponent implements OnDestroy {
  private readonly subs = new SubSink();

  data: Array<ParticipantListingItem> = [];
  queryTyping: ParticipantListingQueryForm;
  bulkDeleting: boolean = false;
  selectedParticipantIds: string[] = [];

  private teams: TeamShared[] = [];
  private taskTypes: TaskTypeShared[] = [];

  @ViewChild(TableComponent) private readonly table: TableComponent<ParticipantListingItem>;

  constructor(
    private readonly router: Router,
    private readonly teamSharedHttpService: TeamSharedHttpService,
    private readonly taskTypeSharedHttpService: TaskTypeSharedHttpService,
    private readonly participantHttpService: ParticipantHttpService,
  ) {}

  ngOnDestroy() {
    this.subs.unsubscribe();
  }

  onQuery(event: QueryEvent<ParticipantListingQueryForm>) {
    this.subs.sink = combineLatest({
      teams: this.teamSharedHttpService.getTeams(),
      taskTypes: this.taskTypeSharedHttpService.getTaskTypes(),
    }).subscribe((d) => {
      this.teams = d.teams.data;
      this.taskTypes = d.taskTypes.data;

      event.execute('participant/queryparticipantlisting', event.query);
    });
  }

  onQueryForm(event: QueryFormEvent<void>) {
    event.execute('participant/getparticipantlistingqueryform', {} as any);
  }

  openAddParticipant() {
    this.router.navigate([RoutingNodes.Participant, RoutingNodes.Link_Create]);
  }

  openEditParticipant(event: TableEditRowEvent<ParticipantListingItem>) {
    this.router.navigate([RoutingNodes.Participant, RoutingNodes.Link_Edit, event.row.id]);
  }

  openParticipantProfile(participantId: string) {
    this.router.navigate([RoutingNodes.Participant, RoutingNodes.Profile, participantId]);
  }

  onSelectParticipants(event: TableSelectRowsEvent<ParticipantListingItem>) {
    this.selectedParticipantIds = event.selectedRows;
  }

  bulkDeleteParticipants() {
    this.bulkDeleting = true;

    this.subs.sink = this.participantHttpService
      .bulkDeleteParticipants(this.selectedParticipantIds)
      .pipe(
        catchError((error) => {
          this.bulkDeleting = false;
          return throwError(() => error);
        }),
      )
      .subscribe((res) => {
        if (res.isSuccess) {
          this.bulkDeleting = false;
          this.selectedParticipantIds = [];
          this.table.refresh();
        }
      });
  }

  renderTeamNames(teamIds: string[]) {
    if (!teamIds) return undefined;

    const names = teamIds.map((id) => {
      const value = this.teams.find((i) => i.id == id);
      return value?.shortName ?? id;
    });

    return names.join(', ');
  }

  renderTaskTypeTitles(taskTypeIds: string[]) {
    if (!taskTypeIds) return undefined;

    const names = taskTypeIds.map((id) => {
      const value = this.taskTypes.find((i) => i.id == id);
      return value?.shortName ?? id;
    });

    return names.join(', ');
  }
}

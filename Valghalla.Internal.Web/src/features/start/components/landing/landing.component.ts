import { Component, OnDestroy } from '@angular/core';
import { QueryEvent, QueryForm, QueryFormEvent } from 'src/shared/query-engine/models/query-form';
import { SingleSelectionFilterValue } from 'src/shared/query-engine/models/single-selection-filter-value';
import { SubSink } from 'subsink';
import { GlobalStateService } from '../../../../app/global-state.service';
import { BuildingEventLogResponse } from '../../models/building-event-log-response';
import { PersonEventLogResponse } from '../../models/person-event-log-response';

interface BuildingEventLogResponseRow extends BuildingEventLogResponse {
  name?: never;
}

interface PersonEventLogResponseRow extends PersonEventLogResponse {
  name?: never;
}

interface BuildingEventLogQueryForm extends QueryForm {
  electionId: string;
  category?: SingleSelectionFilterValue<string>;
}

interface PersonEventLogQueryForm extends QueryForm {
  electionId: string;
  category?: SingleSelectionFilterValue<string>;
}

@Component({
  selector: 'app-landing',
  templateUrl: './landing.component.html',
  styleUrls: ['./landing.component.scss'],
  providers: [],
})
export class LandingComponent implements OnDestroy {
  private subs = new SubSink();
  buildingEventLogData: Array<BuildingEventLogResponseRow> = [];
  personEventLogData: Array<PersonEventLogResponseRow> = [];
  queryBuildingEventLogTyping: BuildingEventLogQueryForm;
  queryPersonEventLogTyping: PersonEventLogQueryForm;
  constructor(private readonly globalStateService: GlobalStateService) {}

  ngOnDestroy(): void {
    this.subs.unsubscribe();
  }
}

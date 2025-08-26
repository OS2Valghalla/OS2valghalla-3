import { Component, OnDestroy, ViewChild } from '@angular/core';
import { TableEditRowEvent } from 'src/shared/models/ux/table';
import { SubSink } from 'subsink';
import { GlobalStateService } from '../../../app/global-state.service';
import { QueryEvent, QueryForm, QueryFormEvent } from 'src/shared/query-engine/models/query-form';
import { WorkLocationListingItem } from './models/work-location-listing-item';
import { Router } from '@angular/router';
import { RoutingNodes } from 'src/shared/constants/routing-nodes';
import { WorkLocationHttpService } from './services/work-location-http.service';
import { TableComponent } from 'src/shared/components/table/table.component';

@Component({
  selector: 'app-admin-work-location',
  templateUrl: './work-location.component.html',
  providers: [WorkLocationHttpService],
})
export class WorkLocationComponent implements OnDestroy {
  private readonly subs = new SubSink();

  loading = true;
  data: Array<WorkLocationListingItem> = [];

  @ViewChild(TableComponent) private readonly table: TableComponent<WorkLocationListingItem>;

  constructor(
    private readonly router: Router,
    private readonly workLocationHttpService: WorkLocationHttpService,
  ) {}

  ngOnDestroy() {
    this.subs.unsubscribe();
  }

  onQuery(event: QueryEvent<QueryForm>) {
    if (!event.query.order) {
      event.query.order = {
        name: 'title',
        descending: false,
      };
    }
    event.execute('administration/worklocation/queryworklocationlisting', event.query);
  }

  onQueryForm(event: QueryFormEvent<void>) {
    event.execute('administration/worklocation/getworklocationlistingqueryform');
  }

  openAddWorkLocation() {
    this.router.navigate([RoutingNodes.Administration, RoutingNodes.WorkLocation, RoutingNodes.Link_Create]);
  }

  openEditWorkLocation(event: TableEditRowEvent<WorkLocationListingItem>) {
    this.router.navigate([
      RoutingNodes.Administration,
      RoutingNodes.WorkLocation,
      RoutingNodes.Link_Edit,
      event.row.id,
    ]);
  }

  deleteWorkLocation(event: TableEditRowEvent<WorkLocationListingItem>) {
    this.subs.sink = this.workLocationHttpService.deleteWorkLocation(event.row.id).subscribe((res) => {
      if (res.isSuccess) {
        this.table.refresh();
      }
    });
  }
}

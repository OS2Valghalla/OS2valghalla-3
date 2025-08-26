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
    // Ensure default ordering
    if (!event.query.order) {
      event.query.order = { name: 'title', descending: false };
    }

    // If we already have data loaded (allData) apply client-side filters first
    if (this.allData && this.allData.length > 0) {
      // Re-apply filters to update this.data
      this.applyElectionFilter();

      const pageSize = this.table?.paginator?.pageSize || event.query.take || 10;
      // If the filtered result fits on a single page, avoid hitting the server again
      if (this.data.length > 0 && this.data.length <= pageSize) {
        // Adjust paginator to reflect filtered size
        const keys = this.data.map(x => (x as any).id) as any[];
        this.table?.setPaginator(keys);
        this.table?.resetPageIndex();
        return; // Skip server query
      }
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
import { Component, OnDestroy, ViewChild } from '@angular/core';
import { TableEditRowEvent } from 'src/shared/models/ux/table';
import { SubSink } from 'subsink';
import { QueryEvent, QueryForm, QueryFormEvent } from 'src/shared/query-engine/models/query-form';
import { AreaListingItem } from './models/area-listing-item';
import { Router } from '@angular/router';
import { RoutingNodes } from 'src/shared/constants/routing-nodes';
import { AreaHttpService } from './services/area-http.service';
import { TableComponent } from 'src/shared/components/table/table.component';

interface AreaListingQueryForm extends QueryForm {
  electionId: string;
}

@Component({
  selector: 'app-admin-area',
  templateUrl: './area.component.html',
  providers: [AreaHttpService],
})
export class AreaComponent implements OnDestroy {
  private readonly subs = new SubSink();

  loading = true;
  data: Array<AreaListingItem> = [];

  @ViewChild(TableComponent) private readonly table: TableComponent<AreaListingItem>;

  constructor(private readonly router: Router, private readonly areaHttpService: AreaHttpService) {}

  ngOnDestroy() {
    this.subs.unsubscribe();
  }

  onQuery(event: QueryEvent<AreaListingQueryForm>) {
    event.execute('administration/area/queryarealisting', event.query);
  }

  onQueryForm(event: QueryFormEvent<void>) {
    event.execute('administration/area/getarealistingqueryform');
  }

  openAddArea() {
    this.router.navigate([RoutingNodes.Administration, RoutingNodes.Area, RoutingNodes.Link_Create]);
  }

  openEditArea(event: TableEditRowEvent<AreaListingItem>) {
    this.router.navigate([RoutingNodes.Administration, RoutingNodes.Area, RoutingNodes.Link_Edit, event.row.id]);
  }

  deleteArea(event: TableEditRowEvent<AreaListingItem>) {
    this.subs.sink = this.areaHttpService.deleteArea(event.row.id).subscribe((res) => {
      if (res.isSuccess) {
        this.table.refresh();
      }
    });
  }
}

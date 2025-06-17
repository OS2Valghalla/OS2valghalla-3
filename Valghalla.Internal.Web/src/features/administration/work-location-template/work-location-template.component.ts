import { Component, OnDestroy, ViewChild } from '@angular/core';
import { TableEditRowEvent } from 'src/shared/models/ux/table';
import { SubSink } from 'subsink';
import { GlobalStateService } from '../../../app/global-state.service';
import { QueryEvent, QueryForm, QueryFormEvent } from 'src/shared/query-engine/models/query-form';
import { WorkLocationTemplateListingItem } from './models/work-location-template-listing-item';
import { Router } from '@angular/router';
import { RoutingNodes } from 'src/shared/constants/routing-nodes';
import { WorkLocationTemplateHttpService } from './services/work-location-template-http.service';
import { TableComponent } from 'src/shared/components/table/table.component';

@Component({
  selector: 'app-admin-work-location-template',
  templateUrl: './work-location-template.component.html',
  providers: [WorkLocationTemplateHttpService],
})
export class WorkLocationTemplateComponent implements OnDestroy {
  private readonly subs = new SubSink();

  loading = true;
  data: Array<WorkLocationTemplateListingItem> = [];

  @ViewChild(TableComponent) private readonly table: TableComponent<WorkLocationTemplateListingItem>;

  constructor(
    private readonly router: Router,
    private readonly workLocationTemplateHttpService: WorkLocationTemplateHttpService,
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
    event.execute('administration/worklocationtemplate/queryworklocationtemplatelisting', event.query);
  }

  onQueryForm(event: QueryFormEvent<void>) {
    event.execute('administration/worklocationtemplate/getworklocationtemplatelistingqueryform');
  }

  openAddWorkLocationTemplate() {
    this.router.navigate([RoutingNodes.Administration, RoutingNodes.WorkLocationTemplate, RoutingNodes.Link_Create]);
  }

  openEditWorkLocationTemplate(event: TableEditRowEvent<WorkLocationTemplateListingItem>) {
    this.router.navigate([
      RoutingNodes.Administration,
      RoutingNodes.WorkLocationTemplate,
      RoutingNodes.Link_Edit,
      event.row.id,
    ]);
  }

  deleteWorkLocationTemplate(event: TableEditRowEvent<WorkLocationTemplateListingItem>) {
    this.subs.sink = this.workLocationTemplateHttpService.deleteWorkLocationTemplate(event.row.id).subscribe((res) => {
      if (res.isSuccess) {
        this.table.refresh();
      }
    });
  }
}

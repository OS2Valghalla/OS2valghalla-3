import { Component, OnDestroy, ViewChild } from '@angular/core';
import { TableEditRowEvent } from 'src/shared/models/ux/table';
import { SubSink } from 'subsink';
import { QueryEvent, QueryForm, QueryFormEvent } from 'src/shared/query-engine/models/query-form';
import { Router } from '@angular/router';
import { RoutingNodes } from 'src/shared/constants/routing-nodes';
import { TableComponent } from 'src/shared/components/table/table.component';
import { TaskTypeTemplateHttpService } from './services/task-type-template-http.service';
import { TaskTypeTemplateListingItem } from './models/task-type-template-listing-item';

@Component({
  selector: 'app-admin-task-type-template',
  templateUrl: './task-type-template.component.html',
  providers: [TaskTypeTemplateHttpService],
})
export class TaskTypeTemplateComponent implements OnDestroy {
  private readonly subs = new SubSink();

  loading = true;
  data: Array<TaskTypeTemplateListingItem> = [];

  @ViewChild(TableComponent) private readonly table: TableComponent<TaskTypeTemplateListingItem>;

  constructor(private readonly router: Router, private readonly taskTypeHttpService: TaskTypeTemplateHttpService) { }

  ngOnDestroy() {
    this.subs.unsubscribe();
  }

  onQuery(event: QueryEvent<QueryForm>) {
    event.execute('administration/tasktypetemplate/querytasktypetemplatelisting', event.query);
  }

  onQueryForm(event: QueryFormEvent<void>) {
    event.execute('administration/tasktypetemplate/gettasktypetemplatelistingqueryform');
  }

  openAddTaskType() {
    this.router.navigate([RoutingNodes.Administration, RoutingNodes.TaskTypeTemplate, RoutingNodes.Link_Create]);
  }

  openEditTaskType(event: TableEditRowEvent<TaskTypeTemplateListingItem>) {
    this.router.navigate([RoutingNodes.Administration, RoutingNodes.TaskTypeTemplate, RoutingNodes.Link_Edit, event.row.id]);
  }

  deleteTaskType(event: TableEditRowEvent<TaskTypeTemplateListingItem>) {
    this.subs.sink = this.taskTypeHttpService.deleteTaskTypeTemplate(event.row.id).subscribe((res) => {
      if (res.isSuccess) {
        this.table.refresh();
      }
    });
  }
}

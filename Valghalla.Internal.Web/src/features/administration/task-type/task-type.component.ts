import { Component, OnDestroy, ViewChild } from '@angular/core';
import { TableEditRowEvent } from 'src/shared/models/ux/table';
import { SubSink } from 'subsink';
import { QueryEvent, QueryForm, QueryFormEvent } from 'src/shared/query-engine/models/query-form';
import { Router } from '@angular/router';
import { RoutingNodes } from 'src/shared/constants/routing-nodes';
import { TableComponent } from 'src/shared/components/table/table.component';
import { TaskTypeHttpService } from './services/task-type-http.service';
import { TaskTypeListingItem } from './models/task-type-listing-item';

@Component({
  selector: 'app-admin-task-type',
  templateUrl: './task-type.component.html',
  providers: [TaskTypeHttpService],
})
export class TaskTypeComponent implements OnDestroy {
  private readonly subs = new SubSink();

  loading = true;
  data: Array<TaskTypeListingItem> = [];

  @ViewChild(TableComponent) private readonly table: TableComponent<TaskTypeListingItem>;

  constructor(private readonly router: Router, private readonly taskTypeHttpService: TaskTypeHttpService) {}

  ngOnDestroy() {
    this.subs.unsubscribe();
  }

  onQuery(event: QueryEvent<QueryForm>) {
    event.execute('administration/tasktype/querytasktypelisting', event.query);
  }

  onQueryForm(event: QueryFormEvent<void>) {
    event.execute('administration/tasktype/gettasktypelistingqueryform');
  }

  openAddTaskType() {
    this.router.navigate([RoutingNodes.Administration, RoutingNodes.TaskType, RoutingNodes.Link_Create]);
  }

  openEditTaskType(event: TableEditRowEvent<TaskTypeListingItem>) {
    this.router.navigate([RoutingNodes.Administration, RoutingNodes.TaskType, RoutingNodes.Link_Edit, event.row.id]);
  }

  deleteTaskType(event: TableEditRowEvent<TaskTypeListingItem>) {
    this.subs.sink = this.taskTypeHttpService.deleteTaskType(event.row.id).subscribe((res) => {
      if (res.isSuccess) {
        this.table.refresh();
      }
    });
  }
}
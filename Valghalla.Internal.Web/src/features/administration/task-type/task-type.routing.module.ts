import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { RoutingNodes } from 'src/shared/constants/routing-nodes';
import { TaskTypeComponent } from '../task-type/task-type.component';
import { TaskTypeItemComponent } from '../task-type/components/task-type-item/task-type-item.component';

const routes: Routes = [
    {
        path: '',
        title: 'app.navigation.task_type.title',
        data: { breadcrumb: 'app.navigation.task_type.title' },
        component: TaskTypeComponent,
      },
    {
        path: RoutingNodes.Link_Create,
        title: 'administration.task_type.task_type_item.page_title.create',
        data: { breadcrumb: 'administration.task_type.task_type_item.page_title.create' },
        component: TaskTypeItemComponent,
    },
    {
        path: RoutingNodes.Link_Edit + '/:id',
        title: 'administration.task_type.task_type_item.page_title.edit',
        data: { breadcrumb: 'administration.task_type.task_type_item.page_title.edit' },
        component: TaskTypeItemComponent,
    },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class TaskTypeRoutingModule {}
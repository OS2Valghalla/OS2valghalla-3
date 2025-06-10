import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { RoutingNodes } from 'src/shared/constants/routing-nodes';
import { TaskTypeTemplateItemComponent } from './components/task-type-template-item/task-type-template-item.component';
import { TaskTypeTemplateComponent } from './task-type-template.component';

const routes: Routes = [
  {
    path: '',
    title: 'app.navigation.task_type_template.title',
    data: { breadcrumb: 'app.navigation.task_type_template.title' },
    component: TaskTypeTemplateComponent,
  },
  {
    path: RoutingNodes.Link_Create,
    title: 'administration.task_type_template.task_type_template_item.page_title.create',
    data: { breadcrumb: 'administration.task_type_template.task_type_template_item.page_title.create' },
    component: TaskTypeTemplateItemComponent,
  },
  {
    path: RoutingNodes.Link_Edit + '/:id',
    title: 'administration.task_type_template.task_type_template_item.page_title.edit',
    data: { breadcrumb: 'administration.task_type_template.task_type_template_item.page_title.edit' },
    component: TaskTypeTemplateItemComponent,
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class TaskTypeTemplateRoutingModule { }
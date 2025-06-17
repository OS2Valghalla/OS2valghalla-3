import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { RoutingNodes } from 'src/shared/constants/routing-nodes';
import { WorkLocationTemplateItemComponent } from './components/work-location-template-item/work-location-template-item.component';
import { WorkLocationTemplateComponent } from './work-location-template.component';

const routes: Routes = [
    {
        path: '',
        title: 'administration.work_location_template.page_title',
        data: { breadcrumb: 'administration.work_location_template.page_title' },
        component: WorkLocationTemplateComponent,
      },
    {
        path: RoutingNodes.Link_Create,
        title: 'administration.work_location.work_location_template_item.page_title.create',
        data: { breadcrumb: 'administration.work_location_template.work_location_template_item.page_title.create' },
        component: WorkLocationTemplateItemComponent,
    },
    {
        path: RoutingNodes.Link_Edit + '/:id',
        title: 'administration.work_location.work_location_template_item.page_title.edit',
        data: { breadcrumb: 'administration.work_location_template.work_location_template_item.page_title.edit' },
        component: WorkLocationTemplateItemComponent,
    },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class WorkLocationTemplateRoutingModule {}
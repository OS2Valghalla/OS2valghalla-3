import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { RoutingNodes } from 'src/shared/constants/routing-nodes';
import { WorkLocationComponent } from '../work-location/work-location.component';
import { WorkLocationItemComponent } from '../work-location/components/work-location-item/work-location-item.component';

const routes: Routes = [
    {
        path: '',
        title: 'administration.work_location.page_title',
        data: { breadcrumb: 'administration.work_location.page_title' },
        component: WorkLocationComponent,
      },
    {
        path: RoutingNodes.Link_Create,
        title: 'administration.work_location.work_location_item.page_title.create',
        data: { breadcrumb: 'administration.work_location.work_location_item.page_title.create' },
        component: WorkLocationItemComponent,
    },
    {
        path: RoutingNodes.Link_Edit + '/:id',
        title: 'administration.work_location.work_location_item.page_title.edit',
        data: { breadcrumb: 'administration.work_location.work_location_item.page_title.edit' },
        component: WorkLocationItemComponent,
    },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class WorkLocationRoutingModule {}
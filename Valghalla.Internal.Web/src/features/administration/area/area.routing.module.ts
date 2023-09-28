import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { RoutingNodes } from 'src/shared/constants/routing-nodes';
import { AreaComponent } from '../area/area.component';
import { AreaItemComponent } from '../area/components/area-item/area-item.component';

const routes: Routes = [
    {
        path: '',
        title: 'administration.area.page_title',
        data: { breadcrumb: 'administration.area.page_title' },
        component: AreaComponent,
      },
    {
        path: RoutingNodes.Link_Create,
        title: 'administration.area.area_item.page_title.create',
        data: { breadcrumb: 'administration.area.area_item.page_title.create' },
        component: AreaItemComponent,
    },
    {
        path: RoutingNodes.Link_Edit + '/:id',
        title: 'administration.area.area_item.page_title.edit',
        data: { breadcrumb: 'administration.area.area_item.page_title.edit' },
        component: AreaItemComponent,
    },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class AreaRoutingModule {}
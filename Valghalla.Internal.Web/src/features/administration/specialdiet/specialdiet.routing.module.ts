import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { RoutingNodes } from 'src/shared/constants/routing-nodes';
import { SpecialDietComponent } from '../specialdiet/specialdiet.component';
import { SpecialDietItemComponent } from '../specialdiet/components/specialdiet-item.component';

const routes: Routes = [
    {
        path: '',
        title: 'administration.specialdiet.page_title',
        data: { breadcrumb: 'administration.specialdiet.page_title' },
        component: SpecialDietComponent,
      },
    {
        path: RoutingNodes.Link_Create,
        title: 'administration.specialdiet.create_page_title',
        data: { breadcrumb: 'administration.specialdiet.create_page_title' },
        component: SpecialDietItemComponent,
    },
    {
        path: RoutingNodes.Link_Edit + '/:id',
        title: 'administration.specialdiet.edit_page_title',
        data: { breadcrumb: 'administration.specialdiet.edit_page_title' },
        component: SpecialDietItemComponent,
    },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class SpecialDietRoutingModule {}
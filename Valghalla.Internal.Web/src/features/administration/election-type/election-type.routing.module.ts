import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { RoutingNodes } from 'src/shared/constants/routing-nodes';
import { ElectionTypeComponent } from '../election-type/election-type.component';
import { ElectionTypeItemComponent } from '../election-type/components/election-type-item/election-type-item.component';

const routes: Routes = [
    {
        path: '',
        title: 'administration.election_type.page_title',
        data: { breadcrumb: 'administration.election_type.page_title' },
        component: ElectionTypeComponent,
      },
    {
        path: RoutingNodes.Link_Create,
        title: 'administration.election_type.create_page_title',
        data: { breadcrumb: 'administration.election_type.create_page_title' },
        component: ElectionTypeItemComponent,
    },
    {
        path: RoutingNodes.Link_Edit + '/:id',
        title: 'administration.election_type.edit_page_title',
        data: { breadcrumb: 'administration.election_type.edit_page_title' },
        component: ElectionTypeItemComponent,
    },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class ElectionTypeRoutingModule {}
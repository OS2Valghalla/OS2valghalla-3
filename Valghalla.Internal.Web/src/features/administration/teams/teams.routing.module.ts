import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { RoutingNodes } from 'src/shared/constants/routing-nodes';
import { TeamsComponent } from '../teams/teams.component';
import { TeamItemComponent } from '../teams/components/team-item/team-item.component';

const routes: Routes = [
    {
        path: '',
        title: 'administration.teams.page_title',
        data: { breadcrumb: 'administration.teams.page_title' },
        component: TeamsComponent,
      },
    {
        path: RoutingNodes.Link_Create,
        title: 'administration.teams.create_page_title',
        data: { breadcrumb: 'administration.teams.create_page_title' },
        component: TeamItemComponent,
    },
    {
        path: RoutingNodes.Link_Edit + '/:id',
        title: 'administration.teams.edit_page_title',
        data: { breadcrumb: 'administration.teams.edit_page_title' },
        component: TeamItemComponent,
    },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class TeamsRoutingModule {}
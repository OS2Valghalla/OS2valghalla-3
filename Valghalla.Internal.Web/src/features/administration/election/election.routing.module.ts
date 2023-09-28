import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { RoutingNodes } from 'src/shared/constants/routing-nodes';
import { ElectionComponent } from '../election/election.component';
import { ElectionItemComponent } from '../election/components/election-item/election-item.component';
import { DuplicateElectionComponent } from '../election/components/duplicate-election/duplicate-election.component';
import { EditElectionCommunicationConfigurationComponent } from '../election/components/edit-election-communication-configuration/edit-election-communication-configuration.component';

const routes: Routes = [
    {
        path: '',
        title: 'administration.election.page_title',
        data: { breadcrumb: 'administration.election.page_title' },
        component: ElectionComponent,
      },
    {
        path: RoutingNodes.Link_Create,
        title: 'administration.election.create_page_title',
        data: { breadcrumb: 'administration.election.create_page_title' },
        component: ElectionItemComponent,
    },
    {
        path: RoutingNodes.Link_Edit + '/:id',
        title: 'administration.election.edit_page_title',
        data: { breadcrumb: 'administration.election.edit_page_title' },
        component: ElectionItemComponent,
    },
    {
      path: RoutingNodes.Link_Edit + '-communication-configuration/:id',
      title: 'administration.election.edit_communication_configuration_page_title',
      data: { breadcrumb: 'administration.election.edit_communication_configuration_page_title' },
      component: EditElectionCommunicationConfigurationComponent,
    },
    {
      path: RoutingNodes.Link_Duplicate + '/:id',
      title: 'administration.election.duplicate_page_title',
      data: { breadcrumb: 'administration.election.duplicate_page_title' },
      component: DuplicateElectionComponent,
    },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class ElectionRoutingModule {}
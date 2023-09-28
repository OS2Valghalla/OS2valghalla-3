import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { RoutingNodes } from 'src/shared/constants/routing-nodes';
import { CommunicationTemplatesComponent } from './components/communication-templates/communication-templates.component';
import { AuthGuard } from 'src/app/auth/auth.guard';
import { CommunicationTemplateItemComponent } from './components/communication-template-item/communication-template-item.component';

const routes: Routes = [
  {
    path: '',
    title: 'app.navigation.communication.communication_templates.title',
    data: { breadcrumb: 'app.navigation.communication.communication_templates.title' },
    component: CommunicationTemplatesComponent,
    canActivate: [AuthGuard],
  },
  {
    path: RoutingNodes.Link_Create,
    title: 'communication.communication_template_item.page_title.create',
    data: { breadcrumb: 'communication.communication_template_item.page_title.create' },
    component: CommunicationTemplateItemComponent,
    canActivate: [AuthGuard],
  },
  {
    path: RoutingNodes.Link_Edit + '/:id',
    title: 'communication.communication_template_item.page_title.edit',
    data: { breadcrumb: 'communication.communication_template_item.page_title.edit' },
    component: CommunicationTemplateItemComponent,
    canActivate: [AuthGuard],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class CommunicationTemplateRoutingModule {}

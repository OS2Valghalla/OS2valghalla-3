import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CommunicationSendMessageComponent } from './components/send-message.component';
import { AuthGuard } from 'src/app/auth/auth.guard';

const routes: Routes = [
  {
    path: '',
    title: 'app.navigation.communication.send_message.title',
    data: { breadcrumb: 'app.navigation.communication.send_message.title' },
    component: CommunicationSendMessageComponent,
    canActivate: [AuthGuard],
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class CommunicationSendMessageRoutingModule {}

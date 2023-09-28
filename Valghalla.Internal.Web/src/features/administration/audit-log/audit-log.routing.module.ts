import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuditLogLandingComponent } from './audit-log.component';

const routes: Routes = [
  {
    path: '',
    title: 'administration.audit_log.page_title',
    data: { breadcrumb: 'administration.audit_log.page_title' },
    component: AuditLogLandingComponent,
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class AuditLogRoutingModule {}

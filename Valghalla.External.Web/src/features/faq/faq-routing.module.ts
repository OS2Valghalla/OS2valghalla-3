import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { FaqLandingComponent } from './landing/landing.component';

const routes: Routes = [
  {
    path: '',
    title: 'faq.landing.page_title',
    component: FaqLandingComponent,
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class FaqRoutingModule {}

import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { MyWorkLocationLandingComponent } from './components/landing.component';

const routes: Routes = [
  {
    path: '',
    title: 'my_work_location.landing.page_title',
    component: MyWorkLocationLandingComponent,
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class MyWorkLocationRoutingModule {}

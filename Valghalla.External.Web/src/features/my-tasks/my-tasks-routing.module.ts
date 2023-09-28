import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { MyTasksLandingComponent } from './components/landing.component';

const routes: Routes = [
  {
    path: '',
    title: 'my_tasks.landing.page_title',
    component: MyTasksLandingComponent,
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class MyTasksRoutingModule {}

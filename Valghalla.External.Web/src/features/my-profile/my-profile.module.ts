import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from 'src/shared/shared.module';
import { TranslocoModule } from '@ngneat/transloco';
import { MyProfileLandingComponent } from './components/landing/landing.component';
import { MyProfileRoutingModule } from './my-profile-routing.module';
import { DkfdsModule } from 'src/shared/dkfds/dkfds.module';
import { ReactiveFormsModule } from '@angular/forms';

@NgModule({
  declarations: [MyProfileLandingComponent],
  imports: [CommonModule, ReactiveFormsModule, MyProfileRoutingModule, SharedModule, TranslocoModule, DkfdsModule],
})
export class MyProfileModule {}

import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LandingComponent } from './components/landing/landing.component';
import { StartRoutingModule } from './start-routing.module';
import { SharedModule } from '../../shared/shared.module';
import { MaterialModule } from '../../shared/material.module';
import { TranslocoModule } from '@ngneat/transloco';



@NgModule({
  declarations: [
    LandingComponent,
  ],
  imports: [
    CommonModule,
    StartRoutingModule,
    SharedModule,
    MaterialModule,
    TranslocoModule,
  ],
})
export class StartModule { }

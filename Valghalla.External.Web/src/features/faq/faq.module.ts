import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from 'src/shared/shared.module';
import { TranslocoModule } from '@ngneat/transloco';
import { FaqLandingComponent } from './landing/landing.component';
import { FaqRoutingModule } from './faq-routing.module';
import { DkfdsModule } from 'src/shared/dkfds/dkfds.module';

@NgModule({
  declarations: [FaqLandingComponent],
  imports: [CommonModule, FaqRoutingModule, SharedModule, TranslocoModule, DkfdsModule],
})
export class FaqModule {}

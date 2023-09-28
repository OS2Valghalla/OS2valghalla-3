import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { TranslocoModule } from '@ngneat/transloco';
import { NgxPaginationModule } from 'ngx-pagination';
import { SpinnerDirective, SpinnerComponent } from './directives/spinner.directive';
import { DkfdsModule } from './dkfds/dkfds.module';
import { FormGroupDirective } from './directives/form/form-group.directive';
import { FormHintDirective } from './directives/form/form-hint.directive';
import { FormLabelDirective } from './directives/form/form-label.directive';
import { FormErrorDirective } from './directives/form/form-error.directive';
import { DateInputComponent } from './components/date-input/date-input.component';
import { AppPaginationComponent } from './components/pagination/pagination.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { FormInputDirective } from './directives/form/form-input.directive';
import { FormControlDirective } from './directives/form/standalone/form-control.directive';
import { FormAriaDescribedByDirective } from './directives/form/standalone/form-aria-describedby.directive';
import { FormSelectDirective } from './directives/form/form-select.directive';
import { FormRadioDirective } from './directives/form/form-radio.directive';
import { FormRadioOptionDirective } from './directives/form/form-radio-option.directive';
import { FormRadioLabelDirective } from './directives/form/form-radio-lable.directive';
import { FormAriaInvalidDirective } from './directives/form/standalone/form-aria-invalid.directive';
import { FormRadioGroupDirective } from './directives/form/form-radio-group.directive';
import { FormDateDirective } from './directives/form/form-date.directive';
import { FormCheckboxGroupDirective } from './directives/form/form-checkbox-group.directive';
import { FormCheckboxOptionDirective } from './directives/form/form-checkbox-option.directive';
import { FormCheckboxDirective } from './directives/form/form-checkbox.directive';
import { FormCheckboxLabelDirective } from './directives/form/form-checkbox-label.directive';
import { ToastComponent } from './components/toast/toast.component';
import { AppTimeSpanPipe } from './pipes/time-span.pipe';
import { AppWeekdayNamePipe } from './pipes/weekday-name.pipe';
import { AppMonthNamePipe } from './pipes/month-name.pipe';

@NgModule({
  declarations: [
    // directives
    SpinnerDirective,
    FormGroupDirective,
    FormHintDirective,
    FormLabelDirective,
    FormErrorDirective,
    FormInputDirective,
    FormDateDirective,
    FormSelectDirective,
    FormRadioDirective,
    FormRadioOptionDirective,
    FormRadioLabelDirective,
    FormRadioGroupDirective,
    FormCheckboxGroupDirective,
    FormCheckboxDirective,
    FormCheckboxOptionDirective,
    FormCheckboxLabelDirective,
    // components
    SpinnerComponent,
    DateInputComponent,
    AppPaginationComponent,
    ToastComponent,
    // pipes
    AppTimeSpanPipe,
    AppWeekdayNamePipe,
    AppMonthNamePipe,
  ],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    DkfdsModule,
    TranslocoModule,
    NgxPaginationModule,
    FormControlDirective,
    FormAriaDescribedByDirective,
    FormAriaInvalidDirective,
  ],
  exports: [
    // directives
    NgxPaginationModule,
    SpinnerDirective,
    FormGroupDirective,
    FormHintDirective,
    FormLabelDirective,
    FormErrorDirective,
    FormInputDirective,
    FormDateDirective,
    FormSelectDirective,
    FormRadioDirective,
    FormRadioOptionDirective,
    FormRadioLabelDirective,
    FormRadioGroupDirective,
    FormCheckboxGroupDirective,
    FormCheckboxDirective,
    FormCheckboxOptionDirective,
    FormCheckboxLabelDirective,
    // components
    DateInputComponent,
    AppPaginationComponent,
    ToastComponent,
    // pipes
    AppTimeSpanPipe,
    AppWeekdayNamePipe,
    AppMonthNamePipe,
  ],
})
export class SharedModule {}

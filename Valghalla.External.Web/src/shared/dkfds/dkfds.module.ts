import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { TranslocoModule } from '@ngneat/transloco';
import { OverflowMenuDirective } from './directives/overflow-menu.directive';
import { TabNavDirective } from './directives/tab-nav.directive';
import { AccordionDirective } from './directives/accordion.directive';
import { AlertDirective } from './directives/alert.directive';
import { InputRegexDirective } from './directives/input-regex.directive';
import { DatePickerDirective } from './directives/date-picker.directive';
import { ErrorSummaryDirective } from './directives/error-summary.directive';
import { CharacterLimitDirective } from './directives/character-limit.directive';
import { ModalDirective } from './directives/modal.directive';
import { RadioToggleGroupDirective } from './directives/radio-toggle-group.directive';
import { CheckboxToggleContentDirective } from './directives/checkbox-toggle-content.directive';
import { TooltipDirective } from './directives/tooltip.directive';
import { ReactiveFormsModule } from '@angular/forms';
import { ToastDirective } from './directives/toast.directive';

@NgModule({
  declarations: [
    OverflowMenuDirective,
    TabNavDirective,
    AccordionDirective,
    AlertDirective,
    InputRegexDirective,
    DatePickerDirective,
    ErrorSummaryDirective,
    CharacterLimitDirective,
    ModalDirective,
    RadioToggleGroupDirective,
    CheckboxToggleContentDirective,
    TooltipDirective,
    ToastDirective,
  ],
  imports: [CommonModule, ReactiveFormsModule, TranslocoModule],
  exports: [
    OverflowMenuDirective,
    TabNavDirective,
    AccordionDirective,
    AlertDirective,
    InputRegexDirective,
    DatePickerDirective,
    ErrorSummaryDirective,
    CharacterLimitDirective,
    ModalDirective,
    RadioToggleGroupDirective,
    CheckboxToggleContentDirective,
    TooltipDirective,
    ToastDirective,
  ],
})
export class DkfdsModule {}

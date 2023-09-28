import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatTooltipModule } from '@angular/material/tooltip';
import { RouterModule } from '@angular/router';
import { TranslocoModule } from '@ngneat/transloco';
import { NgxSkeletonLoaderModule } from 'ngx-skeleton-loader';
import { AppDatePipe } from './pipes/date-time.pipe';
import { AutosaveNumberInputComponent } from './components/autosave-number-input/autosave-number-input.component';
import { CardComponent } from './components/card/card.component';
import { ChipsComponent } from './components/chips-option/chips-option.component';
import { ConfirmationDialogComponent } from './components/confirmation-dialog/confirmation-dialog.component';
import { FieldValueComponent } from './components/field-value/field-value.component';
import { FormDialogComponent } from './components/form-dialog/form-dialog.component';
import { PageMenuItemComponent } from './components/page-menu/page-menu-item.component';
import { PageMenuComponent } from './components/page-menu/page-menu.component';
import { PageTitleComponent } from './components/page-title/page-title.component';
import { RichTextInputComponent } from './components/rich-text-input/rich-text-input.component';
import { RoutingCardComponent } from './components/routing-card/routing-card.component';
import { SideModalComponent } from './components/side-modal/side-modal.component';
import { AppSnackbarComponent } from './components/snackbar/snackbar.component';
import { TableCellDefinitionDirective } from './components/table/directives/cell-def.directive';
import { TableCellDirective } from './components/table/directives/cell.directive';
import { TableColumnDefinitionDirective } from './components/table/directives/column-def.directive';
import { TableHeaderCellDefinitionDirective } from './components/table/directives/header-cell-def.directive';
import { TableHeaderRowDefinitionDirective } from './components/table/directives/header-row-def.directive';
import { TableContextDirective } from './components/table/directives/table-context.directive';
import { TablePanelDefinitionDirective } from './components/table/directives/table-panel-def.directive';
import { TableRowLinkComponent } from './components/table/table-row-link.component';
import { TableComponent } from './components/table/table.component';
import { ValidationMessageComponent } from './components/validation-message/validation-message.component';
import { AppSkeletonComponent, ShowSkeletonDirective } from './directives/skeleton.directive';
import { MaterialModule } from './material.module';
import { BooleanFilterComponent } from './query-engine/components/boolean-filter.component';
import { DynamicQueryComponent } from './query-engine/components/dynamic-query.component';
import { FreeTextSearchComponent } from './query-engine/components/free-text-search.component';
import { MultipleSelectionFilterComponent } from './query-engine/components/multiple-selection-filter.component';
import { QueryContainerComponent } from './query-engine/components/query-container.component';
import { QueryRemovalComponent } from './query-engine/components/query-removal.component';
import { SingleSelectionFilterComponent } from './query-engine/components/single-selection-filter.component';
import { DynamicQueryContentDirective } from './query-engine/directives/dynamic-query-content.directive';
import { QueryContainerContentDirective } from './query-engine/directives/query-container-content.directive';
import { FormPageComponent } from './components/form-page/form-page.component';
import { FileStorageComponent } from './components/file-storage/file-storage.component';
import { WizardComponent } from './components/wizard/wizard.component';
import { WizardStepDirective } from './components/wizard/wizard-step.directive';
import { WizardStepComponent } from './components/wizard/wizard-step.component';
import { MultipleSelectionComponent } from './components/multiple-selection/multiple-selection.component';
import { DateTimeFilterComponent } from './query-engine/components/date-time-filter.component';
import { ParticipantPickerComponent } from './components/participant-picker/participant-picker.component';
import { CardButtonDirective } from './components/card/card-button.directive';
import { AppAuditLogDatePipe } from './pipes/audit-log-time.pipe';

@NgModule({
  declarations: [
    AppDatePipe,
    AppAuditLogDatePipe,
    PageTitleComponent,
    PageMenuComponent,
    PageMenuItemComponent,
    FormDialogComponent,
    FormPageComponent,
    AutosaveNumberInputComponent,
    AppSkeletonComponent,
    ShowSkeletonDirective,
    ConfirmationDialogComponent,
    CardComponent,
    CardButtonDirective,
    AppSnackbarComponent,
    TableComponent,
    TableContextDirective,
    TableColumnDefinitionDirective,
    TableHeaderCellDefinitionDirective,
    TableCellDefinitionDirective,
    TableCellDirective,
    TableHeaderRowDefinitionDirective,
    TablePanelDefinitionDirective,
    TableRowLinkComponent,
    FieldValueComponent,
    ChipsComponent,
    RoutingCardComponent,
    SideModalComponent,
    SingleSelectionFilterComponent,
    MultipleSelectionFilterComponent,
    FreeTextSearchComponent,
    BooleanFilterComponent,
    DateTimeFilterComponent,
    DynamicQueryComponent,
    QueryContainerComponent,
    QueryRemovalComponent,
    DynamicQueryContentDirective,
    QueryContainerContentDirective,
    RichTextInputComponent,
    ValidationMessageComponent,
    FileStorageComponent,
    WizardComponent,
    WizardStepDirective,
    WizardStepComponent,
    MultipleSelectionComponent,
    ParticipantPickerComponent,
  ],
  imports: [CommonModule, MaterialModule, TranslocoModule, NgxSkeletonLoaderModule, RouterModule, MatTooltipModule],
  exports: [
    AppDatePipe,
    AppAuditLogDatePipe,
    PageTitleComponent,
    PageMenuComponent,
    FormDialogComponent,
    FormPageComponent,
    AutosaveNumberInputComponent,
    ShowSkeletonDirective,
    ConfirmationDialogComponent,
    CardComponent,
    CardButtonDirective,
    AppSnackbarComponent,
    TableComponent,
    TableContextDirective,
    TableColumnDefinitionDirective,
    TableHeaderCellDefinitionDirective,
    TableCellDefinitionDirective,
    TableCellDirective,
    TableHeaderRowDefinitionDirective,
    TablePanelDefinitionDirective,
    TableRowLinkComponent,
    FieldValueComponent,
    ChipsComponent,
    RoutingCardComponent,
    SideModalComponent,
    DynamicQueryComponent,
    QueryContainerComponent,
    QueryRemovalComponent,
    DynamicQueryContentDirective,
    QueryContainerContentDirective,
    RichTextInputComponent,
    ValidationMessageComponent,
    FileStorageComponent,
    WizardComponent,
    WizardStepDirective,
    WizardStepComponent,
    MultipleSelectionComponent,
    ParticipantPickerComponent,
  ],
  providers: [{ provide: MAT_DIALOG_DATA, useValue: {} }],
})
export class SharedModule {}

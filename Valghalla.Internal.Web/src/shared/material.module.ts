import { DragDropModule } from '@angular/cdk/drag-drop';
import { Platform } from '@angular/cdk/platform';
import { CommonModule, DATE_PIPE_DEFAULT_OPTIONS } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { LuxonDateAdapter, LuxonDateModule, MAT_LUXON_DATE_FORMATS } from '@angular/material-luxon-adapter';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatButtonModule } from '@angular/material/button';
import { MatButtonToggleModule } from '@angular/material/button-toggle';
import { MatCardModule } from '@angular/material/card';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatChipsModule } from '@angular/material/chips';
import { DateAdapter, MAT_DATE_FORMATS, MAT_DATE_LOCALE } from '@angular/material/core';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MAT_DIALOG_DATA, MatDialogModule } from '@angular/material/dialog';
import { MatDividerModule } from '@angular/material/divider';
import { MatExpansionModule } from '@angular/material/expansion';
import { MAT_FORM_FIELD_DEFAULT_OPTIONS, MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatListModule } from '@angular/material/list';
import { MatMenuModule } from '@angular/material/menu';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatRadioModule } from '@angular/material/radio';
import { MatSelectModule } from '@angular/material/select';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { MatSortModule } from '@angular/material/sort';
import { MatTableModule } from '@angular/material/table';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatStepperModule } from '@angular/material/stepper';
import { MatTabsModule } from '@angular/material/tabs';
import { DateTime } from 'luxon';
import { NgxEditorModule } from 'ngx-editor';
import { NgxFileDropModule } from 'ngx-file-drop';
import { NgxMatTimepickerModule } from 'ngx-mat-timepicker';
import { BreadcrumbModule } from 'xng-breadcrumb';
import { dateFormat } from './constants/date';

class AppDateFormat extends LuxonDateAdapter {
  // read more: https://moment.github.io/luxon/#/parsing
  // eslint-disable-next-line
  override format(date: DateTime, originalDisplayFormat: string) {
    return date.toFormat(dateFormat);
  }

  override getFirstDayOfWeek(): number {
    return 1;
  }
}

@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    MatIconModule,
    MatCardModule,
    MatButtonModule,
    MatButtonToggleModule,
    MatToolbarModule,
    MatSnackBarModule,
    MatMenuModule,
    MatDialogModule,
    MatFormFieldModule,
    MatInputModule,
    MatCheckboxModule,
    MatSelectModule,
    MatDatepickerModule,
    LuxonDateModule,
    MatProgressSpinnerModule,
    MatSidenavModule,
    MatListModule,
    MatTableModule,
    MatSortModule,
    MatPaginatorModule,
    NgxMatTimepickerModule,
    MatDividerModule,
    MatChipsModule,
    MatAutocompleteModule,
    MatRadioModule,
    MatExpansionModule,
    MatProgressBarModule,
    MatStepperModule,
    MatTabsModule,
    DragDropModule,
    NgxFileDropModule,
    NgxEditorModule,
    MatTooltipModule,
    BreadcrumbModule    
  ],
  exports: [
    FormsModule,
    ReactiveFormsModule,
    MatIconModule,
    MatCardModule,
    MatButtonModule,
    MatButtonToggleModule,
    MatToolbarModule,
    MatSnackBarModule,
    MatMenuModule,
    MatDialogModule,
    MatFormFieldModule,
    MatInputModule,
    MatCheckboxModule,
    MatSelectModule,
    MatDatepickerModule,
    LuxonDateModule,
    MatProgressSpinnerModule,
    MatSidenavModule,
    MatListModule,
    MatTableModule,
    MatSortModule,
    MatPaginatorModule,
    NgxMatTimepickerModule,
    MatDividerModule,
    MatChipsModule,
    MatAutocompleteModule,
    MatRadioModule,
    MatExpansionModule,
    MatProgressBarModule,
    MatStepperModule,
    MatTabsModule,
    DragDropModule,
    NgxFileDropModule,
    NgxEditorModule,
    MatTooltipModule,
    BreadcrumbModule
  ],
  providers: [
    { provide: MAT_DATE_LOCALE, useValue: 'sv-SE' },
    {
      provide: DateAdapter,
      useClass: AppDateFormat,
      deps: [MAT_DATE_LOCALE, Platform],
    },
    { provide: MAT_DATE_FORMATS, useValue: MAT_LUXON_DATE_FORMATS },
    {
      provide: DATE_PIPE_DEFAULT_OPTIONS,
      useValue: { dateFormat: dateFormat },
    },
    { provide: MAT_DIALOG_DATA, useValue: {} },
    {
      provide: MAT_FORM_FIELD_DEFAULT_OPTIONS,
      useValue: {
        subscriptSizing: 'dynamic',
      },
    },
  ],
})
export class MaterialModule { }

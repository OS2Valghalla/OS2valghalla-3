import { Component, EventEmitter, Input, OnDestroy, OnInit, Output, ViewChild } from '@angular/core';
import { SubSink } from 'subsink';
import { QueryEvent, QueryForm, QueryFormEvent } from 'src/shared/query-engine/models/query-form';
import { MultipleSelectionFilterValue } from 'src/shared/query-engine/models/multiple-selection-filter-value';
import { TableComponent } from 'src/shared/components/table/table.component';
import { DateTimeFilterValue } from 'src/shared/query-engine/models/date-time-filter-value';
import { BooleanFilterValue } from 'src/shared/query-engine/models/boolean-filter-value';
import { ParticipantSharedListingItem } from 'src/shared/models/participant/participant-shared-listing-item';
import { ParticipantSharedHttpService } from 'src/shared/services/participant-shared-http.service';
import { ParticipantShared } from 'src/shared/models/participant/participant-shared';
import { ControlValueAccessor, FormControl, NG_VALUE_ACCESSOR } from '@angular/forms';
import { TranslocoService } from '@ngneat/transloco';

interface ParticipantSharedListingQueryForm extends QueryForm {
  birthdate?: DateTimeFilterValue;
  teams?: MultipleSelectionFilterValue<string>;
}

@Component({
  selector: 'app-participant-picker',
  templateUrl: './participant-picker.component.html',
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: ParticipantPickerComponent,
      multi: true,
    },
  ],
})
export class ParticipantPickerComponent implements OnInit, OnDestroy, ControlValueAccessor {
  private subs = new SubSink();

  @Input() label: string;

  @Input() multiple: boolean;

  @Input() visible?: boolean = false;

  @Output() submitEvent = new EventEmitter<void>();

  @Output() closeEvent = new EventEmitter<void>();

  queryTyping: ParticipantSharedListingQueryForm;

  data: Array<ParticipantSharedListingItem> = [];

  viewData: Array<ParticipantShared> = [];

  selection: string[] = [];

  loading: boolean = false;

  readonly formData = new FormControl<string[]>([]);

  readonly selectParicipantOption = {
    id: '__alternative__',
    alternative: true,
    name: this.translocoService.translate('shared.participant_picker.select_participant'),
  } as any;

  private onChange: (value: Array<string>) => void;
  private onTouch: (value: Array<string>) => void;

  @ViewChild(TableComponent) table: TableComponent<any>;

  constructor(
    private readonly translocoService: TranslocoService,
    private readonly participantSharedHttpService: ParticipantSharedHttpService,
  ) {}

  ngOnInit(): void {
    this.viewData = this.viewData.concat(this.selectParicipantOption);

    this.subs.sink = this.formData.valueChanges.subscribe((values) => {
      this.selection = values;
      this.onChange(this.selection);
      this.onTouch(this.selection);
    });
  }

  ngOnDestroy(): void {
    this.subs.unsubscribe();
  }

  writeValue(values: string[]): void {
    if (!values || values.length == 0) return;

    this.loading = true;

    this.subs.sink = this.participantSharedHttpService.getParticipants(values).subscribe((res) => {
      this.loading = false;
      this.viewData = res.data.concat(this.selectParicipantOption);
      this.selection = values;
      setTimeout(() => this.formData.setValue(this.selection));
    });
  }

  registerOnChange(fn: any): void {
    this.onChange = fn;
  }

  registerOnTouched(fn: any): void {
    this.onTouch = fn;
  }

  setDisabledState?(isDisabled: boolean): void {}

  isSelected(row: ParticipantSharedListingItem) {
    return this.selection.some((id) => id == row.id);
  }

  onQuery(event: QueryEvent<ParticipantSharedListingQueryForm>) {
    event.execute('shared/participant/queryparticipantsharedlisting', event.query);
  }

  onQueryForm(event: QueryFormEvent<void>) {
    event.execute('shared/participant/getparticipantsharedlistingqueryform', {} as any);
  }

  onSingleSelect(row: ParticipantSharedListingItem) {
    this.selection = [row.id];
  }

  onMultipleToggle(row: ParticipantSharedListingItem) {
    if (this.selection.some((id) => id == row.id)) {
      this.selection = this.selection.filter((id) => id != row.id);
    } else {
      this.selection = [...this.selection, row.id];
    }
  }

  openPariticipantSelectionModal() {
    this.visible = true;
  }

  onClose() {
    this.visible = false;
    this.closeEvent.emit();
  }

  confirmSelection() {
    if (!this.selection || this.selection.length == 0) return;

    this.loading = true;

    this.subs.sink = this.participantSharedHttpService.getParticipants(this.selection).subscribe((res) => {
      this.loading = false;
      this.visible = false;
      this.onChange(this.selection);
      this.onTouch(this.selection);
      this.viewData = res.data.concat(this.selectParicipantOption);
      setTimeout(() => {
        this.formData.setValue(this.selection);
        this.closeEvent.emit();
        this.submitEvent.emit();
      });
    });
  }
}

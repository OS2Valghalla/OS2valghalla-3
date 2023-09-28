import {
  AfterViewInit,
  Component,
  ElementRef,
  EventEmitter,
  HostBinding,
  Input,
  OnDestroy,
  OnInit,
  Output,
  ViewChild,
} from '@angular/core';
import {
  debounceTime,
  distinctUntilChanged,
  filter,
  fromEvent,
  tap,
} from 'rxjs';
import { SubSink } from 'subsink';
import { FreeTextSearchValue } from '../models/free-text-search-value';

@Component({
  selector: 'app-free-text-search',
  template: `
    <mat-form-field class="grow w-full" appearance="fill">
      <mat-label *ngIf="standalone">{{ label | transloco }}</mat-label>
      <input matInput [value]="model?.value" #input />
    </mat-form-field>
  `,
})
export class FreeTextSearchComponent
implements OnInit, AfterViewInit, OnDestroy {
  private subs = new SubSink();

  @Input() label: string;

  @Input() model: FreeTextSearchValue;

  @Input() standalone?: boolean;

  @Input() debounce?: number;

  @Output() modelChange = new EventEmitter<FreeTextSearchValue>();

  @HostBinding('class.flex') flexClass: boolean;

  @ViewChild('input') input: ElementRef;

  ngOnInit(): void {
    if (!this.standalone) {
      this.flexClass = true;
    }
  }

  ngOnDestroy(): void {
    this.subs.unsubscribe();
  }

  ngAfterViewInit(): void {
    this.subs.sink = fromEvent(this.input.nativeElement, 'keyup')
      .pipe(
        filter(Boolean),
        this.debounce ? debounceTime(this.debounce) : debounceTime(500),
        distinctUntilChanged(),
        tap(() => {
          if (!this.input.nativeElement.value) {
            this.model = undefined;
            this.modelChange.emit(this.model);
            return;
          }

          this.model = {
            value: this.input.nativeElement.value,
          };

          this.modelChange.emit(this.model);
        }),
      )
      .subscribe();
  }
}

import { AfterViewInit, Component, ElementRef, EventEmitter, Input, OnDestroy, Output, ViewChild } from '@angular/core';
import { debounceTime, distinctUntilChanged, filter, fromEvent, tap } from 'rxjs';
import { SubSink } from 'subsink';
import { FreeTextSearchValue } from '../models/free-text-search-value';

@Component({
  selector: 'app-free-text-search',
  template: `
    <div class="form-group search">
      <label for="input-type-text-icon" class="sr-only">Søg efter indhold</label>
      <input class="form-input input-char-27" [attr.id]="id" [attr.name]="id" type="search" />
      <button class="button button-search">
        <svg class="icon-svg m-0" focusable="false" aria-hidden="true"><use xlink:href="#search"></use></svg>
        <span class="sr-only">Søg</span>
      </button>
    </div>
  `,
})
export class FreeTextSearchComponent implements AfterViewInit, OnDestroy {
  readonly id = crypto.randomUUID();
  private subs = new SubSink();

  @Input() model: FreeTextSearchValue;

  @Input() debounce?: number;

  @Output() modelChange = new EventEmitter<FreeTextSearchValue>();

  @ViewChild('input') input: ElementRef;

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

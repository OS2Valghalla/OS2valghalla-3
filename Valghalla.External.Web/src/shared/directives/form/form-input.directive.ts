import { AfterViewInit, Directive, ElementRef, Input, OnDestroy, OnInit, Renderer2 } from '@angular/core';
import { FormControlDirective as NgFormControlDirective, Validators } from '@angular/forms';
import { FormControlDirective } from './standalone/form-control.directive';
import { FormAriaDescribedByDirective } from './standalone/form-aria-describedby.directive';
import { SubSink } from 'subsink';
import { datePicker } from 'dkfds';
import { DateTime } from 'luxon';

const INTERNAL_DKFDS_DATE_FORMAT = "yyyy-MM-dd";

@Directive({ selector: '[appFormInput]', hostDirectives: [FormControlDirective, FormAriaDescribedByDirective] })
export class FormInputDirective implements OnInit, OnDestroy, AfterViewInit {
  private readonly subs = new SubSink();
  readonly id = crypto.randomUUID();

  @Input() datePicker: boolean;

  private datePickerDkfdsElement: Element;
  private datePickerDkfdsEvent: any;

  constructor(
    private readonly renderer: Renderer2,
    private readonly elementRef: ElementRef,
    public readonly ngFormControl: NgFormControlDirective,
  ) {}

  ngOnInit(): void {
    this.setAttribute('id', this.id);
    this.setAttribute('name', this.id);
    this.addClass('form-input');

    if (this.ngFormControl.control.hasValidator(Validators.required)) {
      this.setAttribute('required', '');
    }
  }

  ngOnDestroy(): void {
    this.subs.unsubscribe();

    if (this.datePickerDkfdsElement) {
      this.datePickerDkfdsElement.removeEventListener('change', this.datePickerDkfdsEvent);
    }
  }

  ngAfterViewInit(): void {
    if (this.datePicker && !this.datePickerDkfdsElement) {
      const dkfdsDateFormat = datePicker.getDatePickerContext(this.elementRef.nativeElement).dateFormatOption as string;

      const elements = document.getElementsByClassName('date-picker__internal-input');
      for (let i = 0; i < elements.length; i++) {
        const element = elements.item(i);
        if (element.getAttribute('name') == this.id) {
          this.datePickerDkfdsElement = element;
          break;
        }
      }

      this.datePickerDkfdsEvent = (event) => {
        const rawValue = (event.target as any).value;
        const newValue = !rawValue ? rawValue : DateTime.fromFormat(rawValue, INTERNAL_DKFDS_DATE_FORMAT).toUTC().toISO();

        if (newValue != this.ngFormControl.value) {
          this.ngFormControl.control.setValue(newValue);
          this.ngFormControl.control.markAsDirty();
        }
      };

      this.datePickerDkfdsElement.addEventListener('change', this.datePickerDkfdsEvent);
      this.subs.sink = this.ngFormControl.valueChanges.subscribe((value: string) => {
        const newValue = !value ? value : DateTime.fromISO(value).toFormat(INTERNAL_DKFDS_DATE_FORMAT);
        datePicker.setCalendarValue(this.elementRef.nativeElement, newValue, dkfdsDateFormat, true);
      });
    }
  }

  addClass(name: string) {
    this.renderer.addClass(this.elementRef.nativeElement, name);
  }

  removeClass(name: string) {
    this.renderer.removeClass(this.elementRef.nativeElement, name);
  }

  setAttribute(name: string, value: string) {
    this.renderer.setAttribute(this.elementRef.nativeElement, name, value);
  }
}

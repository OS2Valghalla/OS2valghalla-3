import { DatePipe } from '@angular/common';
import { Inject, LOCALE_ID, Pipe, PipeTransform } from '@angular/core';
import { dateTimeFormat } from '../constants/date';

@Pipe({
  name: 'appDateTime',
})
export class AppDatePipe extends DatePipe implements PipeTransform {
  constructor(@Inject(LOCALE_ID) locale: string) {
    super(locale);
  }

  override transform(value: any, format?: string, timezone?: string, locale?: string): any {
    return super.transform(value, dateTimeFormat, timezone, locale);
  }
}

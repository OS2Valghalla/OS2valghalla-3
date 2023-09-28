import { DatePipe } from '@angular/common';
import { Inject, LOCALE_ID, Pipe } from '@angular/core';
import { auditLogDateTimeFormat } from '../constants/date';

@Pipe({
  name: 'appAuditLogDateTime',
})
export class AppAuditLogDatePipe extends DatePipe {
  constructor(@Inject(LOCALE_ID) locale: string) {
    super(locale);
  }

  override transform(value: any, format?: string, timezone?: string, locale?: string): any {
    return super.transform(value, auditLogDateTimeFormat, timezone, locale);
  }
}

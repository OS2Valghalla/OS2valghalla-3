import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'appTimeSpan',
})
export class AppTimeSpanPipe implements PipeTransform {
  transform(value: string, ...args: any[]) {
    if (typeof value == 'string') {
      return value.substring(0, 5);
    }

    return value;
  }
}

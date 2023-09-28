import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'appWeekdayName',
})
export class AppWeekdayNamePipe implements PipeTransform {
  transform(value: any, ...args: any[]) {
    var date = new Date(value);
    switch (date.getDay()) {
      case 0: 
        return 'app.weekday_name.sun';
      case 1: 
        return 'app.weekday_name.mon';
      case 2: 
        return 'app.weekday_name.tue';
      case 3: 
        return 'app.weekday_name.wed';
      case 4: 
        return 'app.weekday_name.thu';
      case 5: 
        return 'app.weekday_name.fri';
      case 6: 
        return 'app.weekday_name.sat';
      default:
        return '';
    }

  }
}

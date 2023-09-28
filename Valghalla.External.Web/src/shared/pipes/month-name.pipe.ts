import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'appMonthName',
})
export class AppMonthNamePipe implements PipeTransform {
  transform(value: any, ...args: any[]) {
    var date = new Date(value);
    switch (date.getMonth()) {
      case 0: 
        return 'app.month_name.jan';
      case 1: 
        return 'app.month_name.feb';
      case 2: 
        return 'app.month_name.mar';
      case 3: 
        return 'app.month_name.apr';
      case 4: 
        return 'app.month_name.may';
      case 5: 
        return 'app.month_name.jun';
      case 6: 
        return 'app.month_name.jul';
      case 7: 
        return 'app.month_name.aug';
      case 8: 
        return 'app.month_name.sep';
      case 9: 
        return 'app.month_name.oct';
      case 10: 
        return 'app.month_name.nov';
      case 11: 
        return 'app.month_name.dec';
      default:
        return '';
    }
  }
}

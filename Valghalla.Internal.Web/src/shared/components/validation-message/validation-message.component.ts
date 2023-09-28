import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-validation-message',
  template: ` <small class="text-red-500">{{ message | transloco : { params: parameter } }}</small> `,
})
export class ValidationMessageComponent {
  @Input() message: string;
  @Input() parameter: any;
}

import { Component } from '@angular/core';
import { RoutingNodes } from 'src/shared/constants/routing-nodes';

@Component({
  selector: 'app-task-rejection-confirmation',
  templateUrl: './task-rejection-confirmation.component.html',
})
export class TaskRejectionConfirmationComponent {
  readonly contactUsUrl: string = '/' + RoutingNodes.ContactUs;
}

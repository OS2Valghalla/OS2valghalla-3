import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { RoutingNodes } from 'src/shared/constants/routing-nodes';

@Component({
  selector: 'app-task-acceptance-cpr-invalid',
  templateUrl: './task-acceptance-cpr-invalid.component.html',
})
export class TaskAcceptanceCprInvalidComponent implements OnInit {
  readonly contactUsUrl: string = '/' + RoutingNodes.ContactUs;

  stepIndicatorVisible: boolean = false;

  constructor(private readonly route: ActivatedRoute) {}

  ngOnInit(): void {
    this.stepIndicatorVisible = this.route.snapshot.queryParamMap.get('stepper') == 'true';
  }
}

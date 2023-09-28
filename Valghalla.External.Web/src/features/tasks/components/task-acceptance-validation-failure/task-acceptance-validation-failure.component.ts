import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { RoutingNodes } from 'src/shared/constants/routing-nodes';

@Component({
  selector: 'app-task-acceptance-validation-failure.',
  templateUrl: './task-acceptance-validation-failure.component.html',
})
export class TaskAcceptanceValidationFailureComponent implements OnInit {
  readonly contactUsUrl: string = '/' + RoutingNodes.ContactUs;

  stepIndicatorVisible: boolean = false;

  ageFailed: boolean = false;
  municipalityFailed: boolean = false;
  citizenshipFailed: boolean = false;
  legalAdultFailed: boolean = false;
  aliveFailed: boolean = false;

  constructor(private readonly route: ActivatedRoute) {}

  ngOnInit(): void {
    const valuesRaw = decodeURIComponent((this.route.snapshot.params as any).values);
    const values = JSON.parse(valuesRaw) as number[];

    this.stepIndicatorVisible = this.route.snapshot.queryParamMap.get('stepper') == 'true';

    this.aliveFailed = values.includes(0);
    this.ageFailed = values.includes(1);
    this.municipalityFailed = values.includes(2);
    this.citizenshipFailed = values.includes(3);
    this.legalAdultFailed = values.includes(4);
  }
}

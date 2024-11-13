import { Component, ContentChildren, EventEmitter, Input, Output, QueryList, ViewChild, OnInit, OnDestroy } from '@angular/core';
import { SubSink } from 'subsink';
import { ReplaySubject, finalize } from 'rxjs';
import { WizardEvent } from 'src/shared/models/ux/wizard';
import { ActivatedRoute, Router } from '@angular/router';
import { CdkStepper, StepperSelectionEvent } from '@angular/cdk/stepper';
import { GlobalStateService } from 'src/app/global-state.service';
import { RoutingNodes } from 'src/shared/constants/routing-nodes';
import { ElectionShared } from 'src/shared/models/election/election-shared';
import { WizardStepComponent } from './wizard-step.component';

@Component({
  selector: 'app-wizard',
  templateUrl: './wizard.component.html',
})
export class WizardComponent implements OnInit, OnDestroy {
  private readonly subs = new SubSink();
  private readonly state = new ReplaySubject<void>();

  @Input() wizardTitle: string;

  @Input() updateWizardTitle: string;

  @Input() submitButtonText: string;

  @Input() deleteButtonTooltip: string;

  @Output() create = new EventEmitter<WizardEvent>();

  @Output() update = new EventEmitter<WizardEvent>();

  @Output() delete = new EventEmitter<WizardEvent>();

  @Output() onCancelClick = new EventEmitter<void>();

  @Output() onStepEnter = new EventEmitter<StepperSelectionEvent>();

  @Input() hideDeleteButton?: boolean;

  @Input() disableDeleteButton?: boolean;

  @Input() hideOkButton?: boolean;

  @Input() submitOnLastStep?: boolean;

  submitting: boolean = false;
  deleting: boolean = false;

  itemId?: string;
  election?: ElectionShared;
  state$ = this.state.asObservable();

  @ContentChildren(WizardStepComponent, { descendants: false }) readonly wizardSteps!: QueryList<WizardStepComponent>;
  @ViewChild(CdkStepper) readonly stepper: CdkStepper;

  constructor(
    private readonly router: Router,
    private readonly route: ActivatedRoute,
    private readonly globalStateService: GlobalStateService,
  ) {}

  ngOnInit(): void {
    this.itemId = this.route.snapshot.paramMap.get(RoutingNodes.Id);
    this.subs.sink = this.globalStateService.election$.subscribe((election) => {
      this.election = election;
      this.state.next();
    });
  }

  ngOnDestroy(): void {
    this.subs.unsubscribe();
    this.state.complete();
  }

  isUpdateWizard() {
    return !!this.itemId;
  }

  isWizardValid() {
    return !this.wizardSteps.some((step) => !step.formGroup.disabled && !step.formGroup.valid);
  }

  isWizardPristine() {
    return !this.wizardSteps.some((step) => !step.formGroup.pristine);
  }

  isLastStepRequired() {
    return this.submitOnLastStep && this.stepper.selectedIndex != this.wizardSteps.length - 1;
  }

  navigate(path?: any[]) {
    if (path) {
      this.router.navigate(path);
    } else {
      const path = this.isUpdateWizard() ? '../../' : '../';
      this.router.navigate([path], { relativeTo: this.route });
    }
  }

  onStepChange(event: StepperSelectionEvent) {
    this.onStepEnter.emit(event);
  }

  onSubmit() {
    if (!this.isWizardValid() || this.isLastStepRequired()) return;

    this.submitting = true;
    this.wizardSteps.forEach((step) => {
      step.formGroup.disable();
    });

    const formPageEvent: WizardEvent = {
      pipe: (observable) =>
        observable.pipe(
          finalize(() => {
            this.submitting = false;
            this.wizardSteps.forEach((step) => {
              step.formGroup.enable();
            });
          }),
        ),
    };

    if (this.isUpdateWizard()) {
      this.update.emit(formPageEvent);
    } else {
      this.create.emit(formPageEvent);
    }
  }

  onDelete() {
    if (!this.isUpdateWizard()) return;

    this.deleting = true;
    this.wizardSteps.forEach((step) => {
      step.formGroup.disable();
    });

    const formPageEvent: WizardEvent = {
      pipe: (observable) =>
        observable.pipe(
          finalize(() => {
            this.deleting = false;
            this.wizardSteps.forEach((step) => {
              step.formGroup.enable();
            });
          }),
        ),
    };

    this.delete.emit(formPageEvent);
  }

  onCancel() {
    this.onCancelClick.emit();
  }
}

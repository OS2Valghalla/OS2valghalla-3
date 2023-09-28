import { Component, EventEmitter, Input, OnDestroy, OnInit, Output } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ReplaySubject, finalize } from 'rxjs';
import { GlobalStateService } from 'src/app/global-state.service';
import { RoutingNodes } from 'src/shared/constants/routing-nodes';
import { ElectionShared } from 'src/shared/models/election/election-shared';
import { FormPageEvent } from 'src/shared/models/ux/form-page';
import { SubSink } from 'subsink';

@Component({
  selector: 'app-form-page',
  templateUrl: './form-page.component.html',
})
export class FormPageComponent implements OnInit, OnDestroy {
  private readonly subs = new SubSink();
  private readonly state = new ReplaySubject<void>();

  @Input() formTitle: string;

  @Input() updateFormTitle: string;

  // to make form control work as ng-content, we need workaround solution by using same directive formGroup
  @Input() formGroup: FormGroup;

  @Output() create = new EventEmitter<FormPageEvent>();

  @Output() update = new EventEmitter<FormPageEvent>();

  @Output() delete = new EventEmitter<FormPageEvent>();

  @Output() onCancelClick = new EventEmitter<void>();

  @Input() hideDeleteButton?: boolean;

  submitting: boolean = false;
  deleting: boolean = false;

  itemId?: string;
  election?: ElectionShared;
  state$ = this.state.asObservable();

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

  isUpdateForm() {
    return !!this.itemId;
  }

  navigate(path?: any[]) {
    if (path) {
      this.router.navigate(path);
    } else {
      const path = this.isUpdateForm() ? '../../' : '../';
      this.router.navigate([path], { relativeTo: this.route });
    }
  }

  onSubmit() {
    if (!this.formGroup.valid) return;

    this.submitting = true;
    this.formGroup.disable();

    const formPageEvent: FormPageEvent = {
      pipe: (observable) =>
        observable.pipe(
          finalize(() => {
            this.submitting = false;
            this.formGroup.enable();
          }),
        ),
    };

    if (this.isUpdateForm()) {
      this.update.emit(formPageEvent);
    } else {
      this.create.emit(formPageEvent);
    }
  }

  onDelete() {
    if (!this.isUpdateForm()) return;

    this.deleting = true;
    this.formGroup.disable();

    const formPageEvent: FormPageEvent = {
      pipe: (observable) =>
        observable.pipe(
          finalize(() => {
            this.deleting = false;
            this.formGroup.enable();
          }),
        ),
    };

    this.delete.emit(formPageEvent);
  }

  onCancel() {
    this.onCancelClick.emit();
  }
}

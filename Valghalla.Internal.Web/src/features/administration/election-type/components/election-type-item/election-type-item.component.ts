import { AfterViewInit, ChangeDetectorRef, Component, OnDestroy, ViewChild } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { ElectionType } from '../../models/election-type';
import { CreateElectionTypeRequest } from '../../models/create-election-type-request';
import { UpdateElectionTypeRequest } from '../../models/update-election-type-request';
import { ElectionTypeHttpService } from '../../services/election-type-http.service';
import { SubSink } from 'subsink';
import { ElectionValidationRules } from '../../../../../shared/constants/election-validation-rules';
import { FormPageComponent } from 'src/shared/components/form-page/form-page.component';
import { FormPageEvent } from 'src/shared/models/ux/form-page';

@Component({
  selector: 'app-admin-election-type-item',
  templateUrl: './election-type-item.component.html',
  providers: [ElectionTypeHttpService],
})
export class ElectionTypeItemComponent implements AfterViewInit, OnDestroy {
  private readonly subs = new SubSink();

  loading = true;
  item: ElectionType;
  selectedValidationRuleIds: Array<string> = [];

  readonly validationRules = [
    { id: ElectionValidationRules.Age18, title: 'administration.election_type.validation_rule.age_18' },
    {
      id: ElectionValidationRules.MunicipalRequirement,
      title: 'administration.election_type.validation_rule.municipal_requirement',
    },
    { id: ElectionValidationRules.DanishCitizen, title: 'administration.election_type.validation_rule.danish_citizen' },
    { id: ElectionValidationRules.Disempowered, title: 'administration.election_type.validation_rule.disempowered' },
  ];

  readonly form = this.formBuilder.group({
    title: ['', Validators.required],
  });

  @ViewChild(FormPageComponent) private readonly formPage: FormPageComponent;

  constructor(
    private readonly changeDetectorRef: ChangeDetectorRef,
    private readonly formBuilder: FormBuilder,
    private readonly electionTypeHttpService: ElectionTypeHttpService,
  ) {}

  ngAfterViewInit(): void {
    this.subs.sink = this.formPage.state$.subscribe(() => {
      if (!this.formPage.isUpdateForm()) {
        this.loading = false;
        this.changeDetectorRef.detectChanges();
        return;
      }

      this.subs.sink = this.electionTypeHttpService.getElectionType(this.formPage.itemId).subscribe((res) => {
        this.loading = false;
        this.item = res.data;

        if (res.data) {
          this.selectedValidationRuleIds = res.data.validationRuleIds;
          this.form.setValue({
            title: res.data.title,
          });
        }
      });
    });
  }

  ngOnDestroy(): void {
    this.subs.unsubscribe();
  }

  createElectionType(event: FormPageEvent) {
    const createRequest: CreateElectionTypeRequest = {
      title: this.form.value.title,
      validationRuleIds: this.selectedValidationRuleIds,
    };

    this.subs.sink = event.pipe(this.electionTypeHttpService.createElectionType(createRequest)).subscribe((res) => {
      if (res.isSuccess) {
        this.formPage.navigate();
      }
    });
  }

  updateElectionType(event: FormPageEvent) {
    const updateRequest: UpdateElectionTypeRequest = {
      id: this.formPage.itemId,
      title: this.form.value.title,
      validationRuleIds: this.selectedValidationRuleIds,
    };

    this.subs.sink = event.pipe(this.electionTypeHttpService.updateElectionType(updateRequest)).subscribe((res) => {
      if (res.isSuccess) {
        this.formPage.navigate();
      }
    });
  }

  deleteElectionType(event: FormPageEvent) {
    this.subs.sink = event
      .pipe(this.electionTypeHttpService.deleteElectionType(this.formPage.itemId))
      .subscribe((res) => {
        if (res.isSuccess) {
          this.formPage.navigate();
        }
      });
  }

  cancel() {
    this.formPage.navigate();
  }

  updateCheckedValidationRule(validationRuleId, event) {
    if (event.checked) {
      if (this.selectedValidationRuleIds.indexOf(validationRuleId) < 0) {
        this.selectedValidationRuleIds.push(validationRuleId);
        this.form.markAsDirty();
      }
    } else {
      if (this.selectedValidationRuleIds.indexOf(validationRuleId) >= 0) {
        this.selectedValidationRuleIds.splice(this.selectedValidationRuleIds.indexOf(validationRuleId), 1);
        this.form.markAsDirty();
      }
    }
  }
}

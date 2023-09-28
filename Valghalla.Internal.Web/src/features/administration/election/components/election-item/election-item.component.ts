import { AfterViewInit, ChangeDetectorRef, Component, OnDestroy, ViewChild } from '@angular/core';
import { AbstractControl, FormBuilder, Validators, ValidationErrors, FormControl } from '@angular/forms';
import { CreateElectionRequest, CreateElectionTaskTypeCommunicationTemplateRequest } from 'src/features/administration/election/models/create-election-request';
import { UpdateElectionRequest } from 'src/features/administration/election/models/update-election-request';
import { ElectionHttpService } from 'src/features/administration/election/services/election-http.service';
import { SubSink } from 'subsink';
import { SELECTED_ELECTION_TO_WORK_ON } from 'src/shared/constants/election';
import { combineLatest } from 'rxjs';
import { WizardComponent } from 'src/shared/components/wizard/wizard.component';
import { WizardEvent } from 'src/shared/models/ux/wizard';
import { ElectionTypeShared } from 'src/shared/models/election-type/election-type-shared';
import { ElectionTypeSharedHttpService } from 'src/shared/services/election-type-shared-http.service';
import { ElectionDetails } from '../../models/election-details';
import { CommunicationTemplateListingItem } from '../../../../communication/communication-template/models/communication-template-listing-item';
import { WorkLocationShared } from 'src/shared/models/work-location/work-location-shared';
import { WorkLocationSharedHttpService } from 'src/shared/services/work-location-shared-http.service';
import { CommunicationTemplateShared } from 'src/shared/models/communication/communication-template-shared';
import { CommunicationSharedHttpService } from 'src/shared/services/communication-shared-http.service';
import { TaskTypeShared } from 'src/shared/models/task-type/task-type-shared';
import { DefaultCommunicationTemplates } from 'src/shared/constants/default-communication-templates';
import { TaskTypeSharedHttpService } from 'src/shared/services/task-type-shared-http.service';
import { MatCheckboxChange } from '@angular/material/checkbox';

interface TaskTypeWithTemplates extends TaskTypeShared {
  confirmationOfRegistration_Template?: CommunicationTemplateShared;
  confirmationOfCancellation_Template?: CommunicationTemplateShared;
  invitation_Template?: CommunicationTemplateShared;
  invitationReminder_Template?: CommunicationTemplateShared;
  taskReminder_Template?: CommunicationTemplateShared;
  retractedInvitation_Template?: CommunicationTemplateShared;
}

@Component({
  selector: 'app-admin-election-item',
  templateUrl: './election-item.component.html',
  providers: [ElectionHttpService],
})
export class ElectionItemComponent implements AfterViewInit, OnDestroy {
  private readonly subs = new SubSink();

  @ViewChild(WizardComponent) readonly wizard: WizardComponent;

  constructor(
    private readonly formBuilder: FormBuilder,
    private readonly changeDetectorRef: ChangeDetectorRef,
    private readonly electionHttpService: ElectionHttpService,    
    private readonly electionTypeSharedHttpService: ElectionTypeSharedHttpService,
    private readonly workLocationSharedHttpService: WorkLocationSharedHttpService,
    private readonly communicationSharedHttpService: CommunicationSharedHttpService,
    private readonly taskTypeSharedHttpService: TaskTypeSharedHttpService,
  ) {}

  loading = true;
  item: ElectionDetails;
  electionTypes: ElectionTypeShared[] = [];
  workLocations: WorkLocationShared[] = [];
  taskTypes: TaskTypeWithTemplates[] = [];
  communicationTemplates: CommunicationTemplateShared[] = [];

  readonly formInfo = this.formBuilder.group({
    title: ['', Validators.required],
    electionTypeId: ['', Validators.required],
    lockPeriod: [4, [Validators.required, Validators.min(1)]],
  });

  readonly formDate = this.formBuilder.group({
    electionStartDate: [undefined as Date, Validators.required],
    electionEndDate: [undefined as Date, Validators.required],
    electionDate: [undefined as Date, Validators.required],
  });

  readonly formWorkLocation = this.formBuilder.group({
    workLocationIds: [undefined as string[], Validators.required],
  });

  readonly formCommunicationTemplates = this.formBuilder.group({
    confirmationOfRegistration_TemplateType: [undefined],
    confirmationOfRegistration_Template: [ undefined as CommunicationTemplateShared, Validators.required],
    confirmationOfCancellation_TemplateType: [undefined],
    confirmationOfCancellation_Template: [undefined as CommunicationTemplateShared, Validators.required],
    invitation_TemplateType: [undefined],
    invitation_Template: [undefined as CommunicationTemplateShared, Validators.required],
    invitationReminder_TemplateType: [undefined],
    invitationReminder_Template: [undefined as CommunicationTemplateShared],
    taskReminder_TemplateType: [undefined],
    taskReminder_Template: [undefined as CommunicationTemplateShared],
    retractedInvitation_TemplateType: [undefined],
    retractedInvitation_Template: [undefined as CommunicationTemplateShared],
  });

  ngAfterViewInit(): void {
    this.subs.sink = combineLatest({
      wizardState: this.wizard.state$,
      electionTypes: this.electionTypeSharedHttpService.getElectionTypes(),
      workLocations: this.workLocationSharedHttpService.getWorkLocations(),
      taskTypes: this.taskTypeSharedHttpService.getTaskTypes(),
      communicationTemplates: this.communicationSharedHttpService.getCommunicationTemplates(),
    }).subscribe((v) => {
      this.electionTypes = v.electionTypes.data;
      this.workLocations = v.workLocations.data;
      this.taskTypes = v.taskTypes.data;
      this.communicationTemplates = v.communicationTemplates.data;
      
      this.setSelectedTemplate(this.communicationTemplates.filter(t => t.id == DefaultCommunicationTemplates.ConfirmationOfRegistrationStringId)[0], this.formCommunicationTemplates.controls.confirmationOfRegistration_TemplateType, this.formCommunicationTemplates.controls.confirmationOfRegistration_Template);
      this.setSelectedTemplate(this.communicationTemplates.filter(t => t.id == DefaultCommunicationTemplates.ConfirmationOfCancellationStringId)[0], this.formCommunicationTemplates.controls.confirmationOfCancellation_TemplateType, this.formCommunicationTemplates.controls.confirmationOfCancellation_Template);
      this.setSelectedTemplate(this.communicationTemplates.filter(t => t.id == DefaultCommunicationTemplates.InvitationStringId)[0], this.formCommunicationTemplates.controls.invitation_TemplateType, this.formCommunicationTemplates.controls.invitation_Template);
      this.setSelectedTemplate(this.communicationTemplates.filter(t => t.id == DefaultCommunicationTemplates.InvitationReminderStringId)[0], this.formCommunicationTemplates.controls.invitationReminder_TemplateType, this.formCommunicationTemplates.controls.invitationReminder_Template);
      this.setSelectedTemplate(this.communicationTemplates.filter(t => t.id == DefaultCommunicationTemplates.TaskReminderStringId)[0], this.formCommunicationTemplates.controls.taskReminder_TemplateType, this.formCommunicationTemplates.controls.taskReminder_Template);
      this.setSelectedTemplate(this.communicationTemplates.filter(t => t.id == DefaultCommunicationTemplates.RetractedInvitationStringId)[0], this.formCommunicationTemplates.controls.retractedInvitation_TemplateType, this.formCommunicationTemplates.controls.retractedInvitation_Template);

      this.taskTypes.forEach(taskType => {
        taskType.confirmationOfRegistration_Template = undefined;
        taskType.confirmationOfCancellation_Template = undefined;
        taskType.invitation_Template = undefined;
        taskType.invitationReminder_Template = undefined;
        taskType.taskReminder_Template = undefined;
        taskType.retractedInvitation_Template = undefined;
      });

      if (!this.wizard.isUpdateWizard()) {
        this.formDate.addValidators([this.validateElectionDate]);

        this.loading = false;
        this.changeDetectorRef.detectChanges();

        if (this.electionTypes.length > 0) {
          this.formInfo.controls.electionTypeId.setValue(this.electionTypes[0].id);
        }

        return;
      }

      this.subs.sink = this.electionHttpService.getElection(this.wizard.itemId).subscribe((res) => {
        this.loading = false;
        this.item = res.data;

        if (res.data) {
          this.formInfo.controls.electionTypeId.disable();
          this.formDate.controls.electionStartDate.disable();
          this.formDate.controls.electionEndDate.disable();
          this.formDate.controls.electionDate.disable();
          this.formInfo.setValue({
            title: res.data.title,
            electionTypeId: res.data.electionTypeId,
            lockPeriod: res.data.lockPeriod,
          });

          this.formDate.setValue({
            electionStartDate: res.data.electionStartDate,
            electionEndDate: res.data.electionEndDate,
            electionDate: res.data.electionDate,
          });

          this.formWorkLocation.setValue({
            workLocationIds: res.data.workLocationIds,
          });
        }
      });
    });
  }

  ngOnDestroy(): void {
    this.subs.unsubscribe();
  }

  setSelectedTemplate(selectedTemplate: CommunicationTemplateListingItem, formTemplateTypeFieldName: FormControl, formTemplateFieldName: FormControl) {
    if (selectedTemplate && selectedTemplate.id) {
        formTemplateTypeFieldName.setValue(selectedTemplate.templateType);
        var found = this.communicationTemplates.filter(t => t.id == selectedTemplate.id);
        if (found && found.length > 0) {
            formTemplateFieldName.setValue(found[0]);
        }
    }
  }

  validateElectionDate(control: AbstractControl): ValidationErrors | null {
    if (!control || !control.get('electionStartDate') || !control.get('electionStartDate').value || !control.get('electionEndDate') || !control.get('electionEndDate').value || !control.get('electionDate') || !control.get('electionDate').value) {
      if (!control.get('electionDate') || !control.get('electionDate').value) {
        control.get('electionDate').setErrors({required: true});
      }
      else {
        control.get('electionDate').setErrors(null);
      }
      return null;
    }
    
    if (control.get('electionStartDate').value.valueOf() > control.get('electionDate').value.valueOf() || control.get('electionDate').value.valueOf() > control.get('electionEndDate').value.valueOf()) {
      control.get('electionDate').setErrors({invalidElectionDate: true});
      return { invalidElectionDate: {value: true} };
      
    }

    control.get('electionDate').setErrors(null);
    return null;
  }

  isWorkLocationChecked(workLocation: WorkLocationShared) {
    return this.formWorkLocation.value.workLocationIds && this.formWorkLocation.value.workLocationIds.some((id) => id == workLocation.id);
  }

  toggleWorkLocation(event: MatCheckboxChange, workLocationId: string) {
    const values = this.formWorkLocation.value.workLocationIds ? this.formWorkLocation.value.workLocationIds.filter((id) => id != workLocationId) : [];

    if (event.checked) {
      values.push(workLocationId);
    }

    this.formWorkLocation.setValue({
      workLocationIds: values,
    });
    this.formWorkLocation.markAsDirty();
  }

  createElection(event: WizardEvent) {
    var electionTaskTypeCommunicationTemplates: Array<CreateElectionTaskTypeCommunicationTemplateRequest> = [];
    this.taskTypes.forEach(taskType => {
      if (this.hasSpecificTemplate(taskType)) {
        electionTaskTypeCommunicationTemplates.push({
          taskTypeId: taskType.id,
          confirmationOfRegistrationCommunicationTemplateId: taskType.confirmationOfRegistration_Template?.id,
          confirmationOfCancellationCommunicationTemplateId: taskType.confirmationOfCancellation_Template?.id,
          invitationCommunicationTemplateId: taskType.invitation_Template?.id,
          invitationReminderCommunicationTemplateId: taskType.invitationReminder_Template?.id,
          taskReminderCommunicationTemplateId: taskType.taskReminder_Template?.id,
          retractedInvitationCommunicationTemplateId: taskType.retractedInvitation_Template?.id,
        })
      }
    });

    const createRequest: CreateElectionRequest = {
      title: this.formInfo.value.title,
      electionTypeId: this.formInfo.value.electionTypeId,
      lockPeriod: this.formInfo.value.lockPeriod,
      electionStartDate: this.formDate.value.electionStartDate,
      electionEndDate: this.formDate.value.electionEndDate,
      electionDate: this.formDate.value.electionDate,
      workLocationIds: this.formWorkLocation.value.workLocationIds,
      confirmationOfRegistrationCommunicationTemplateId: this.formCommunicationTemplates.value.confirmationOfRegistration_Template.id,
      confirmationOfCancellationCommunicationTemplateId: this.formCommunicationTemplates.value.confirmationOfCancellation_Template.id,
      invitationCommunicationTemplateId: this.formCommunicationTemplates.value.invitation_Template.id,
      invitationReminderCommunicationTemplateId: this.formCommunicationTemplates.value.invitationReminder_Template?.id,
      taskReminderCommunicationTemplateId: this.formCommunicationTemplates.value.taskReminder_Template?.id,
      retractedInvitationCommunicationTemplateId: this.formCommunicationTemplates.value.retractedInvitation_Template?.id,
      electionTaskTypeCommunicationTemplates: electionTaskTypeCommunicationTemplates
    };
    
    this.subs.sink = event.pipe(this.electionHttpService.createElection(createRequest)).subscribe((res) => {
      if (res.isSuccess) {
        this.wizard.navigate();
      }
    });
  }

  updateElection(event: WizardEvent) {
    const updateRequest: UpdateElectionRequest = {
      id: this.wizard.itemId,
      title: this.formInfo.value.title,
      lockPeriod: this.formInfo.value.lockPeriod,
    };
    
    this.subs.sink = event.pipe(this.electionHttpService.updateElection(updateRequest)).subscribe((res) => {
      if (res.isSuccess) {
        this.wizard.navigate();
      }
    });
  }

  deleteElection(event: WizardEvent) {
    this.subs.sink = event.pipe(this.electionHttpService.deleteElection(this.wizard.itemId)).subscribe((res) => {
      if (res.isSuccess) {
        this.wizard.navigate();
      }
    });
  };

  cancel() {
    this.wizard.navigate();
  }
  
  hasSpecificTemplate(taskType: TaskTypeWithTemplates) {
    if ((taskType.confirmationOfRegistration_Template && taskType.confirmationOfRegistration_Template.id)
    || (taskType.confirmationOfCancellation_Template && taskType.confirmationOfCancellation_Template.id)
    || (taskType.invitation_Template && taskType.invitation_Template.id)
    || (taskType.invitationReminder_Template && taskType.invitationReminder_Template.id)
    || (taskType.taskReminder_Template && taskType.taskReminder_Template.id)
    || (taskType.retractedInvitation_Template && taskType.retractedInvitation_Template.id)
    ) {
      return true;
    }

    return false;
  }

  checkIfDisableDeleteElectionButton = () => {
    return this.item && (this.item.id == localStorage.getItem(SELECTED_ELECTION_TO_WORK_ON) || this.item.active);
  }

  getDeleteButtonTooltip = () => {
    if (!this.item || !this.item.id) return '';

    return this.item.id == localStorage.getItem(SELECTED_ELECTION_TO_WORK_ON) ? 'administration.election.messages.cannot_delete_selected_election' : (this.item.active ? 'administration.election.messages.cannot_delete_active_election' : '');
  }
}

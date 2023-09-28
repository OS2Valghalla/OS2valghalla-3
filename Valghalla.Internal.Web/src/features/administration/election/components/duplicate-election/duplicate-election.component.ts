import { AfterViewInit, ChangeDetectorRef, Component, OnDestroy, ViewChild } from '@angular/core';
import { AbstractControl, FormBuilder, Validators, ValidationErrors, FormControl } from '@angular/forms';
import { DuplicateElectionRequest, DuplicateElectionTaskTypeCommunicationTemplateRequest } from 'src/features/administration/election/models/duplicate-election-request';
import { UpdateElectionRequest } from 'src/features/administration/election/models/update-election-request';
import { ElectionHttpService } from 'src/features/administration/election/services/election-http.service';
import { SubSink } from 'subsink';
import { combineLatest } from 'rxjs';
import { WizardComponent } from 'src/shared/components/wizard/wizard.component';
import { WizardEvent } from 'src/shared/models/ux/wizard';
import { ElectionTypeShared } from 'src/shared/models/election-type/election-type-shared';
import { ElectionTypeSharedHttpService } from 'src/shared/services/election-type-shared-http.service';
import { ElectionDetails } from '../../models/election-details';
import { CommunicationTemplateListingItem } from '../../../../communication/communication-template/models/communication-template-listing-item';
import { WorkLocationShared } from 'src/shared/models/work-location/work-location-shared';
import { WorkLocationSharedHttpService } from 'src/shared/services/work-location-shared-http.service';
import { CommunicationTemplateTypes } from 'src/shared/constants/communication-template-types';
import { CommunicationTemplateShared } from 'src/shared/models/communication/communication-template-shared';
import { CommunicationSharedHttpService } from 'src/shared/services/communication-shared-http.service';
import { TaskTypeShared } from 'src/shared/models/task-type/task-type-shared';
import { DefaultCommunicationTemplates } from 'src/shared/constants/default-communication-templates';
import { TaskTypeSharedHttpService } from 'src/shared/services/task-type-shared-http.service';
import { MatCheckboxChange } from '@angular/material/checkbox';

interface TaskTypeWithTemplates extends TaskTypeShared {
  confirmationOfRegistration_Template?: CommunicationTemplateShared;
  confirmationOfRegistration_TemplateType?: CommunicationTemplateTypes;
  confirmationOfCancellation_Template?: CommunicationTemplateShared;
  confirmationOfCancellation_TemplateType?: CommunicationTemplateTypes;
  invitation_Template?: CommunicationTemplateShared;
  invitation_TemplateType?: CommunicationTemplateTypes;
  invitationReminder_Template?: CommunicationTemplateShared;
  invitationReminder_TemplateType?: CommunicationTemplateTypes;
  taskReminder_Template?: CommunicationTemplateShared;
  taskReminder_TemplateType?: CommunicationTemplateTypes;
  retractedInvitation_Template?: CommunicationTemplateShared;
  retractedInvitation_TemplateType?: CommunicationTemplateTypes;
}

@Component({
  selector: 'app-admin-duplicate-election',
  templateUrl: './duplicate-election.component.html',
  providers: [ElectionHttpService],
})
export class DuplicateElectionComponent implements AfterViewInit, OnDestroy {
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

  sourceDaysBeforeElectionDate = 0;
  sourceDaysAfterElectionDate = 0;

  readonly formInfo = this.formBuilder.group({
    title: ['', Validators.required],
    electionTypeId: ['', Validators.required],
    lockPeriod: [4, [Validators.required, Validators.min(1)]],
  });

  readonly formDate = this.formBuilder.group({
    electionDate: [undefined as Date, Validators.required],
    daysBeforeElectionDate: [0, [Validators.required, Validators.min(0)]],
    daysAfterElectionDate: [0, [Validators.required, Validators.min(0)]],
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
    invitationReminder_Template: [undefined as CommunicationTemplateShared, Validators.required],
    taskReminder_TemplateType: [undefined],
    taskReminder_Template: [undefined as CommunicationTemplateShared, Validators.required],
    retractedInvitation_TemplateType: [undefined],
    retractedInvitation_Template: [undefined as CommunicationTemplateShared, Validators.required],
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

      if (!this.wizard.isUpdateWizard()) {
        this.loading = false;
        this.changeDetectorRef.detectChanges();

        if (this.electionTypes.length > 0) {
          this.formInfo.controls.electionTypeId.setValue(this.electionTypes[0].id);
        }

        return;
      }

      this.subs.sink = this.electionHttpService.getElection(this.wizard.itemId).subscribe((res) => {        
        this.item = res.data;
        if (res.data) {
          this.formInfo.setValue({
            title: res.data.title + ' - kopi',
            electionTypeId: res.data.electionTypeId,
            lockPeriod: res.data.lockPeriod,
          });

          this.sourceDaysBeforeElectionDate = (new Date(res.data.electionDate)).getDate() - (new Date(res.data.electionStartDate)).getDate();
          this.sourceDaysAfterElectionDate = (new Date(res.data.electionEndDate)).getDate() - (new Date(res.data.electionDate)).getDate();
          this.formDate.controls.daysBeforeElectionDate.setValue(this.sourceDaysBeforeElectionDate);
          this.formDate.controls.daysAfterElectionDate.setValue(this.sourceDaysAfterElectionDate);

          this.formWorkLocation.setValue({
            workLocationIds: res.data.workLocationIds, 
          });

          this.formWorkLocation.markAsDirty();

          this.subs.sink = this.electionHttpService.getElectionCommunicationConfigurations(this.wizard.itemId).subscribe((res) => {    
            if (res.data) {
                this.setSelectedTemplate(res.data.confirmationOfRegistrationCommunicationTemplate, this.formCommunicationTemplates.controls.confirmationOfRegistration_TemplateType, this.formCommunicationTemplates.controls.confirmationOfRegistration_Template);
                this.setSelectedTemplate(res.data.confirmationOfCancellationCommunicationTemplate, this.formCommunicationTemplates.controls.confirmationOfCancellation_TemplateType, this.formCommunicationTemplates.controls.confirmationOfCancellation_Template);
                this.setSelectedTemplate(res.data.invitationCommunicationTemplate, this.formCommunicationTemplates.controls.invitation_TemplateType, this.formCommunicationTemplates.controls.invitation_Template);
                this.setSelectedTemplate(res.data.invitationReminderCommunicationTemplate, this.formCommunicationTemplates.controls.invitationReminder_TemplateType, this.formCommunicationTemplates.controls.invitationReminder_Template);
                this.setSelectedTemplate(res.data.taskReminderCommunicationTemplate, this.formCommunicationTemplates.controls.taskReminder_TemplateType, this.formCommunicationTemplates.controls.taskReminder_Template);
                this.setSelectedTemplate(res.data.retractedInvitationCommunicationTemplate, this.formCommunicationTemplates.controls.retractedInvitation_TemplateType, this.formCommunicationTemplates.controls.retractedInvitation_Template);
    
                this.taskTypes.forEach(taskType => {
                    var found = res.data.electionTaskTypeCommunicationTemplates.filter(t => t.taskTypeId == taskType.id);
                    if (found && found.length > 0) {                        
                        var electionTaskTypeCommunicationTemplate = found[0];
                        this.setTaskTypeSelectedTemplate(taskType, electionTaskTypeCommunicationTemplate.confirmationOfRegistrationCommunicationTemplate, 'confirmationOfRegistration_Template');
                        this.setTaskTypeSelectedTemplate(taskType, electionTaskTypeCommunicationTemplate.confirmationOfCancellationCommunicationTemplate, 'confirmationOfCancellation_Template');
                        this.setTaskTypeSelectedTemplate(taskType, electionTaskTypeCommunicationTemplate.invitationCommunicationTemplate, 'invitation_Template');
                        this.setTaskTypeSelectedTemplate(taskType, electionTaskTypeCommunicationTemplate.invitationReminderCommunicationTemplate, 'invitationReminder_Template');
                        this.setTaskTypeSelectedTemplate(taskType, electionTaskTypeCommunicationTemplate.taskReminderCommunicationTemplate, 'taskReminder_Template');
                        this.setTaskTypeSelectedTemplate(taskType, electionTaskTypeCommunicationTemplate.retractedInvitationCommunicationTemplate, 'retractedInvitation_Template');
                    }
                });

                this.loading = false;
            }
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

  setTaskTypeSelectedTemplate(taskType: TaskTypeWithTemplates, selectedTemplate: CommunicationTemplateListingItem, taskTypeTemplatePropertyName: string) {
    if (selectedTemplate && selectedTemplate.id) {            
        var found = this.communicationTemplates.filter(t => t.id == selectedTemplate.id);
        if (found && found.length > 0) {
            taskType[taskTypeTemplatePropertyName + 'Type'] = selectedTemplate.templateType;
            taskType[taskTypeTemplatePropertyName] = found[0];
        }
    }
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

  duplicateElection(event: WizardEvent) {
    var electionTaskTypeCommunicationTemplates: Array<DuplicateElectionTaskTypeCommunicationTemplateRequest> = [];
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

    const request: DuplicateElectionRequest = {
      sourceElectionId: this.wizard.itemId,
      title: this.formInfo.value.title,
      electionTypeId: this.formInfo.value.electionTypeId,
      lockPeriod: this.formInfo.value.lockPeriod,
      daysBeforeElectionDate: this.formDate.value.daysBeforeElectionDate,
      daysAfterElectionDate: this.formDate.value.daysAfterElectionDate,
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
    
    this.subs.sink = event.pipe(this.electionHttpService.duplicateElection(request)).subscribe((res) => {
      if (res.isSuccess) {
        this.wizard.navigate();
      }
    });
  }

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
}

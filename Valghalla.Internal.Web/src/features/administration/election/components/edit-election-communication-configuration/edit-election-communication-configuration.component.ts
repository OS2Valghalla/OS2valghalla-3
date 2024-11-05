import { AfterViewInit, Component, OnDestroy, ViewChild } from '@angular/core';
import { FormBuilder, Validators, FormControl } from '@angular/forms';
import { SubSink } from 'subsink';
import { combineLatest } from 'rxjs';
import { FormPageComponent } from 'src/shared/components/form-page/form-page.component';
import { FormPageEvent } from 'src/shared/models/ux/form-page';
import { CommunicationTemplateListingItem } from '../../../../communication/communication-template/models/communication-template-listing-item';
import { CommunicationTemplateTypes } from 'src/shared/constants/communication-template-types';
import { CommunicationTemplateShared } from 'src/shared/models/communication/communication-template-shared';
import { CommunicationSharedHttpService } from 'src/shared/services/communication-shared-http.service';
import { TaskTypeShared } from 'src/shared/models/task-type/task-type-shared';
import { TaskTypeSharedHttpService } from 'src/shared/services/task-type-shared-http.service';
import { ElectionHttpService } from '../../services/election-http.service';
import { ElectionCommunicationConfigurations } from '../../models/election-communication-configurations';
import { UpdateElectionCommunicationConfigurationsRequest, UpdateElectionTaskTypeCommunicationTemplateRequest } from '../..//models/update-election-communication-configurations-request';

interface TaskTypeWithTemplates extends TaskTypeShared {
    confirmationOfRegistration_TemplateType?: CommunicationTemplateTypes;
    confirmationOfRegistration_Template?: CommunicationTemplateShared;
    confirmationOfCancellation_TemplateType?: CommunicationTemplateTypes;
    confirmationOfCancellation_Template?: CommunicationTemplateShared;
    invitation_TemplateType?: CommunicationTemplateTypes;
    invitation_Template?: CommunicationTemplateShared;
    invitationReminder_TemplateType?: CommunicationTemplateTypes;
    invitationReminder_Template?: CommunicationTemplateShared;
    taskReminder_TemplateType?: CommunicationTemplateTypes;
    taskReminder_Template?: CommunicationTemplateShared;
    retractedInvitation_TemplateType?: CommunicationTemplateTypes;
    retractedInvitation_Template?: CommunicationTemplateShared;
    removedFromTask_TemplateType?: CommunicationTemplateTypes;
    removedFromTask_Template?: CommunicationTemplateShared;
    removedByValidation_TemplateType?: CommunicationTemplateTypes;
    removedByValidation_Template?: CommunicationTemplateShared;
  }

@Component({
    selector: 'app-admin-edit-election-communication-configuration',
    templateUrl: './edit-election-communication-configuration.component.html',
    providers: [ElectionHttpService],
  })
  export class EditElectionCommunicationConfigurationComponent implements AfterViewInit, OnDestroy {
    private readonly subs = new SubSink();
  
    loading = true;

    taskTypes: TaskTypeWithTemplates[] = [];

    communicationTemplates: CommunicationTemplateShared[] = [];

    item: ElectionCommunicationConfigurations;

    readonly formCommunicationTemplates = this.formBuilder.group({
        confirmationOfRegistration_TemplateType: [undefined],
        confirmationOfRegistration_Template: [undefined as CommunicationTemplateShared, Validators.required],
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
        removedFromTask_TemplateType: [undefined],
        removedFromTask_Template: [undefined as CommunicationTemplateShared, Validators.required],
        removedByValidation_TemplateType: [undefined],
        removedByValidation_Template: [undefined as CommunicationTemplateShared, Validators.required],
    });
    
    @ViewChild(FormPageComponent) private readonly formPage: FormPageComponent;

    constructor(
        private readonly formBuilder: FormBuilder,
        private readonly electionHttpService: ElectionHttpService,
        private readonly communicationSharedHttpService: CommunicationSharedHttpService,
        private readonly taskTypeSharedHttpService: TaskTypeSharedHttpService,
      ) {}
    
      ngAfterViewInit(): void {
        this.subs.sink = combineLatest({
          taskTypes: this.taskTypeSharedHttpService.getTaskTypes(),
          communicationTemplates: this.communicationSharedHttpService.getCommunicationTemplates(),
        }).subscribe((v) => {
          this.communicationTemplates = v.communicationTemplates.data;        
    
          this.subs.sink = this.electionHttpService.getElectionCommunicationConfigurations(this.formPage.itemId).subscribe((res) => {    
            if (res.data) {
                this.item = res.data;
                this.setSelectedTemplate(this.item.confirmationOfRegistrationCommunicationTemplate, this.formCommunicationTemplates.controls.confirmationOfRegistration_TemplateType, this.formCommunicationTemplates.controls.confirmationOfRegistration_Template);
                this.setSelectedTemplate(this.item.confirmationOfCancellationCommunicationTemplate, this.formCommunicationTemplates.controls.confirmationOfCancellation_TemplateType, this.formCommunicationTemplates.controls.confirmationOfCancellation_Template);
                this.setSelectedTemplate(this.item.invitationCommunicationTemplate, this.formCommunicationTemplates.controls.invitation_TemplateType, this.formCommunicationTemplates.controls.invitation_Template);
                this.setSelectedTemplate(this.item.invitationReminderCommunicationTemplate, this.formCommunicationTemplates.controls.invitationReminder_TemplateType, this.formCommunicationTemplates.controls.invitationReminder_Template);
                this.setSelectedTemplate(this.item.taskReminderCommunicationTemplate, this.formCommunicationTemplates.controls.taskReminder_TemplateType, this.formCommunicationTemplates.controls.taskReminder_Template);
                this.setSelectedTemplate(this.item.retractedInvitationCommunicationTemplate, this.formCommunicationTemplates.controls.retractedInvitation_TemplateType, this.formCommunicationTemplates.controls.retractedInvitation_Template);
                this.setSelectedTemplate(this.item.removedFromTaskCommunicationTemplate, this.formCommunicationTemplates.controls.removedFromTask_TemplateType, this.formCommunicationTemplates.controls.removedFromTask_Template);
                this.setSelectedTemplate(this.item.removedByValidationCommunicationTemplate, this.formCommunicationTemplates.controls.removedByValidation_TemplateType, this.formCommunicationTemplates.controls.removedByValidation_Template);

                v.taskTypes.data.forEach(taskType => {
                    const taskTypeWithTemplate: TaskTypeWithTemplates = {
                        id: taskType.id,
                        title: taskType.title
                    };
                    const found = this.item.electionTaskTypeCommunicationTemplates.filter(t => t.taskTypeId == taskType.id);
                    if (found && found.length > 0) {                        
                        const electionTaskTypeCommunicationTemplate = found[0];
                        this.setTaskTypeSelectedTemplate(taskTypeWithTemplate, electionTaskTypeCommunicationTemplate.confirmationOfRegistrationCommunicationTemplate, 'confirmationOfRegistration_Template');
                        this.setTaskTypeSelectedTemplate(taskTypeWithTemplate, electionTaskTypeCommunicationTemplate.confirmationOfCancellationCommunicationTemplate, 'confirmationOfCancellation_Template');
                        this.setTaskTypeSelectedTemplate(taskTypeWithTemplate, electionTaskTypeCommunicationTemplate.invitationCommunicationTemplate, 'invitation_Template');
                        this.setTaskTypeSelectedTemplate(taskTypeWithTemplate, electionTaskTypeCommunicationTemplate.invitationReminderCommunicationTemplate, 'invitationReminder_Template');
                        this.setTaskTypeSelectedTemplate(taskTypeWithTemplate, electionTaskTypeCommunicationTemplate.taskReminderCommunicationTemplate, 'taskReminder_Template');
                        this.setTaskTypeSelectedTemplate(taskTypeWithTemplate, electionTaskTypeCommunicationTemplate.retractedInvitationCommunicationTemplate, 'retractedInvitation_Template');
                        this.setTaskTypeSelectedTemplate(taskTypeWithTemplate, electionTaskTypeCommunicationTemplate.removedFromTaskCommunicationTemplate, 'removedFromTask_Template');
                        this.setTaskTypeSelectedTemplate(taskTypeWithTemplate, electionTaskTypeCommunicationTemplate.removedByValidationCommunicationTemplate, 'removedByValidation_Template');
                    }
                    this.taskTypes.push(taskTypeWithTemplate);
                });
            }

            this.loading = false;
          });
        });
      }
    
      ngOnDestroy(): void {
        this.subs.unsubscribe();
      }

      setSelectedTemplate(selectedTemplate: CommunicationTemplateListingItem, formTemplateTypeFieldName: FormControl, formTemplateFieldName: FormControl) {
        if (selectedTemplate && selectedTemplate.id) {
            formTemplateTypeFieldName.setValue(selectedTemplate.templateType);
            const found = this.communicationTemplates.filter(t => t.id == selectedTemplate.id);
            if (found && found.length > 0) {
                formTemplateFieldName.setValue(found[0]);
            }
        }
      }

      setTaskTypeSelectedTemplate(taskType: TaskTypeWithTemplates, selectedTemplate: CommunicationTemplateListingItem, taskTypeTemplatePropertyName: string) {
        if (selectedTemplate && selectedTemplate.id) {            
            const found = this.communicationTemplates.filter(t => t.id == selectedTemplate.id);
            if (found && found.length > 0) {
                taskType[taskTypeTemplatePropertyName + 'Type'] = selectedTemplate.templateType;
                taskType[taskTypeTemplatePropertyName] = found[0];
            }
        }
      }

      hasSpecificTemplate(taskType: TaskTypeWithTemplates) {
        if ((taskType.confirmationOfRegistration_Template && taskType.confirmationOfRegistration_Template.id)
        || (taskType.confirmationOfCancellation_Template && taskType.confirmationOfCancellation_Template.id)
        || (taskType.invitation_Template && taskType.invitation_Template.id)
        || (taskType.invitationReminder_Template && taskType.invitationReminder_Template.id)
        || (taskType.taskReminder_Template && taskType.taskReminder_Template.id)
        || (taskType.retractedInvitation_Template && taskType.retractedInvitation_Template.id)
        || (taskType.removedFromTask_Template && taskType.removedFromTask_Template.id)
        || (taskType.removedByValidation_Template && taskType.removedByValidation_Template.id)
        ) {
          return true;
        }
    
        return false;
      }

      updateElection(event: FormPageEvent) {
        const electionTaskTypeCommunicationTemplates: Array<UpdateElectionTaskTypeCommunicationTemplateRequest> = [];
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
                    removedFromTaskCommunicationTemplateId: taskType.removedFromTask_Template?.id,
                    removedByValidationCommunicationTemplateId: taskType.removedByValidation_Template?.id,
                })
            }
        });

        const updateRequest: UpdateElectionCommunicationConfigurationsRequest = {
            id: this.formPage.itemId,
            confirmationOfRegistrationCommunicationTemplateId: this.formCommunicationTemplates.value.confirmationOfRegistration_Template.id,
            confirmationOfCancellationCommunicationTemplateId: this.formCommunicationTemplates.value.confirmationOfCancellation_Template.id,
            invitationCommunicationTemplateId: this.formCommunicationTemplates.value.invitation_Template.id,
            invitationReminderCommunicationTemplateId: this.formCommunicationTemplates.value.invitationReminder_Template?.id,
            taskReminderCommunicationTemplateId: this.formCommunicationTemplates.value.taskReminder_Template?.id,
            retractedInvitationCommunicationTemplateId: this.formCommunicationTemplates.value.retractedInvitation_Template?.id,
            removedFromTaskCommunicationTemplateId: this.formCommunicationTemplates.value.removedFromTask_Template?.id,
            removedByValidationCommunicationTemplateId: this.formCommunicationTemplates.value.removedByValidation_Template?.id,
            electionTaskTypeCommunicationTemplates: electionTaskTypeCommunicationTemplates
          };

        this.subs.sink = event.pipe(this.electionHttpService.updateElectionCommunicationConfigurations(updateRequest)).subscribe((res) => {
            if (res.isSuccess) {
                this.formPage.navigate();
            }
        });
      }

      cancel() {
        this.formPage.navigate();
      }
    }
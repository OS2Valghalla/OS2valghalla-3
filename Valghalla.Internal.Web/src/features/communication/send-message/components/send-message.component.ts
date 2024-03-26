import { Component, OnDestroy, ViewChild, AfterViewInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { StepperSelectionEvent } from '@angular/cdk/stepper';
import { SubSink } from 'subsink';
import { switchMap } from 'rxjs';
import { TranslocoService } from '@ngneat/transloco';
import { Router } from '@angular/router';
import { MatPaginator } from '@angular/material/paginator'
import { MatTableDataSource } from '@angular/material/table';
import { RichTextEditorOptions } from 'src/shared/models/ux/rich-text-editor-options';
import { WizardComponent } from 'src/shared/components/wizard/wizard.component';
import { WizardEvent } from 'src/shared/models/ux/wizard';
import { FileStorageComponent } from 'src/shared/components/file-storage/file-storage.component';
import { FileReference } from "src/shared/models/file-storage/file-reference";
import { ElectionShared } from 'src/shared/models/election/election-shared';
import { TeamShared } from 'src/shared/models/team/team-shared';
import { WorkLocationShared } from 'src/shared/models/work-location/work-location-shared';
import { TaskTypeShared } from 'src/shared/models/task-type/task-type-shared';
import { GlobalStateService } from 'src/app/global-state.service';
import { CommunicationTemplateTypes } from 'src/shared/constants/communication-template-types';
import { CommunicationTemplateShared } from 'src/shared/models/communication/communication-template-shared';
import { CommunicationSharedHttpService } from 'src/shared/services/communication-shared-http.service';
import { FilteredTasksHttpService } from '../../../tasks/services/filtered-tasks-http.service';
import { CommunicationHttpService } from '../../communication-template/services/communication-http.service';

@Component({
  selector: 'app-communication-send-message',
  templateUrl: './send-message.component.html',
  styleUrls: ['../../../../shared/components/table/table.component.scss'],
  providers: [CommunicationHttpService, FilteredTasksHttpService],
})
export class CommunicationSendMessageComponent implements AfterViewInit, OnDestroy {
  private readonly subs = new SubSink();

  loading = true;

  loadingParticipants = true;

  sending = false;

  electionDates: Array<Date> = [];

  teams: Array<TeamShared> = [];

  workLocations: Array<WorkLocationShared> = [];

  taskTypes: Array<TaskTypeShared> = [];

  dataSource: MatTableDataSource<any>;

  recipientsCount = 0;

  @ViewChild(MatPaginator) readonly paginator: MatPaginator;

  readonly templateTypes = [
    { id: CommunicationTemplateTypes.DigitalPost, title: 'communication.template_type.digital_post' },
    { id: CommunicationTemplateTypes.Email, title: 'communication.template_type.email' },
    { id: CommunicationTemplateTypes.SMS, title: 'communication.template_type.sms' },
  ];

  readonly tokens = ['name', 'election', 'work_location', 'work_location_address', 'task_type', 'task_date', 'task_start', 'task_end', 'task_type_description', 'payment', 'days', 'municipality', 'invitation', 'contact', 'external_web'];

  readonly pageContentSettings: RichTextEditorOptions = {
    isRichText: true,
    heightInPixel: 300,
    removeStyleWhenPasting: true,
  };

  readonly formInfo = this.formBuilder.group({
    templateType: [CommunicationTemplateTypes.DigitalPost, Validators.required],
  });

  readonly formContent = this.formBuilder.group({
    selectedTemplate: [undefined as CommunicationTemplateShared],
    subject: ['', [Validators.required, Validators.maxLength(256)]],
    content: ['', Validators.required],
    plainTextContent: [''],
  });

  readonly formRecipients = this.formBuilder.group({
    selectedWorkLocationIds: undefined,
    selectedTeamIds: undefined,
    selectedTaskTypeIds: undefined,
    selectedDates: undefined,
    selectedTaskStatusIds: undefined
  });

  election?: ElectionShared;

  communicationTemplateFileReferences: FileReference[];

  previewSubject: string = '';

  previewContent: string = '';

  preparingPreviewContent = false;

  selectionChanged = false;

  communicationTemplates_DigitalPost: CommunicationTemplateShared[] = [];
  communicationTemplates_Email: CommunicationTemplateShared[] = [];
  communicationTemplates_SMS: CommunicationTemplateShared[] = [];

  @ViewChild(WizardComponent) readonly wizard: WizardComponent;
  @ViewChild(FileStorageComponent) private readonly fileStorage: FileStorageComponent;

  constructor(
    private translocoService: TranslocoService,
    private globalStateService: GlobalStateService,
    private router: Router,
    private formBuilder: FormBuilder,
    private filteredTasksHttpService: FilteredTasksHttpService,
    private communicationSharedHttpService: CommunicationSharedHttpService,
    private communicationHttpService: CommunicationHttpService,
  ) {}

  ngOnDestroy() {
    this.subs.unsubscribe();
  }

  ngAfterViewInit(): void {
    this.subs.sink = this.globalStateService.election$.subscribe((election) => {
      this.election = election;
      if (this.election) {
        this.subs.sink = this.communicationSharedHttpService.getCommunicationTemplates().subscribe((res) => {
          this.communicationTemplates_DigitalPost = res.data.filter(t => t.templateType == CommunicationTemplateTypes.DigitalPost);
          this.communicationTemplates_Email = res.data.filter(t => t.templateType == CommunicationTemplateTypes.Email);
          this.communicationTemplates_SMS = res.data.filter(t => t.templateType == CommunicationTemplateTypes.SMS);
          this.loading = false;
          this.subs.sink = this.filteredTasksHttpService.getTasksFiltersOptions(this.election.id).subscribe((res) => {
            this.teams = res.data.teams;
            this.workLocations = res.data.workLocations;
            this.taskTypes = res.data.taskTypes;
            var tasksDate: Date = new Date(res.data.electionStartDate);
            while (tasksDate <= new Date(res.data.electionEndDate)) {
                this.electionDates.push(new Date(tasksDate));
                tasksDate.setDate(tasksDate.getDate() + 1);
            }
            this.subs.sink = this.communicationHttpService.getParticipantsForSendingGroupMessage({ electionId: this.election.id, filters: { teamIds: [], workLocationIds: [], taskTypeIds: [], taskStatuses: [], taskDates: []} }).subscribe((res) => {            
              this.dataSource = new MatTableDataSource<any>(res.data);
              this.dataSource.paginator = this.paginator;
              this.recipientsCount = res.data.length;
              this.loadingParticipants = false;
            });
          });        
        });
      }
    });
  }

  templateTypeChanged() {  
    this.formContent.controls.selectedTemplate.setValue(undefined);
    if (this.formInfo.value.templateType != CommunicationTemplateTypes.SMS) {
      if (this.formInfo.value.templateType == CommunicationTemplateTypes.DigitalPost) {
        this.formContent.controls.subject.setValidators([Validators.required, Validators.maxLength(256)]);
      }
      else {
        this.formContent.controls.subject.setValidators(Validators.required);
      } 
      this.formContent.controls.subject.updateValueAndValidity();
      this.formContent.controls.content.setValidators(Validators.required);
      this.formContent.controls.content.updateValueAndValidity();
      this.formContent.controls.plainTextContent.clearValidators();
      this.formContent.controls.plainTextContent.updateValueAndValidity();
    } else {
      this.formContent.controls.subject.clearValidators();
      this.formContent.controls.subject.updateValueAndValidity();
      this.formContent.controls.content.clearValidators();
      this.formContent.controls.content.updateValueAndValidity();
      this.formContent.controls.plainTextContent.setValidators(Validators.required);
      this.formContent.controls.plainTextContent.updateValueAndValidity();
    }
  }

  isEmptyContent() {
    if ((this.formInfo.value.templateType != CommunicationTemplateTypes.SMS && this.formContent.controls.content.touched && (!this.formContent.controls.content.value || this.formContent.controls.content.value == '<p></p>')) || (this.formInfo.value.templateType == CommunicationTemplateTypes.SMS && this.formContent.controls.plainTextContent.touched && !this.formContent.controls.plainTextContent.value)) return true;

    return false;
  }

  onStepEnter(event: StepperSelectionEvent) {
    if (event.selectedIndex == 3) {
      this.preparingPreviewContent = true;
      this.previewSubject = this.formContent.controls.subject.value;
      this.previewContent = this.formInfo.value.templateType != CommunicationTemplateTypes.SMS ? this.formContent.controls.content.value : this.formContent.controls.plainTextContent.value;

      this.tokens.sort((a, b) => { return -1 * (a.length - b.length); }).forEach((token) => {
        if (this.formInfo.value.templateType != CommunicationTemplateTypes.SMS) {
          this.previewSubject = (this.previewSubject as any).replaceAll('!' + token, this.translocoService.translate('communication.tokens_preview.' + token));
        }
        this.previewContent = (this.previewContent as any).replaceAll('!' + token, this.translocoService.translate('communication.tokens_preview.' + token));
      });

      this.preparingPreviewContent = false;
    }
  }

  onFiltersToggle(panelOpen: boolean) {
    if (!panelOpen && this.selectionChanged) {
      this.loadingParticipants = true;

      var request = {
        electionId: this.election.id,
        filters: {
            teamIds: this.formRecipients.controls.selectedTeamIds.value ? this.formRecipients.controls.selectedTeamIds.value : [],
            workLocationIds: this.formRecipients.controls.selectedWorkLocationIds.value ? this.formRecipients.controls.selectedWorkLocationIds.value : [],
            taskTypeIds: this.formRecipients.controls.selectedTaskTypeIds.value ? this.formRecipients.controls.selectedTaskTypeIds.value : [],
            taskDates: this.formRecipients.controls.selectedDates.value ? this.formRecipients.controls.selectedDates.value : [],
            taskStatuses: this.formRecipients.controls.selectedTaskStatusIds.value ? this.formRecipients.controls.selectedTaskStatusIds.value : []
        }
      };

      this.subs.sink = this.communicationHttpService.getParticipantsForSendingGroupMessage(request).subscribe((res) => {            
        this.dataSource = new MatTableDataSource<any>(res.data);
        this.dataSource.paginator = this.paginator;
        this.recipientsCount = res.data.length;
        this.loadingParticipants = false;
      });
      this.selectionChanged = false;
    }
  }

  onFiltersChanged() {
    this.selectionChanged = true;
  }

  sendMessage(event: WizardEvent) {
    this.sending = true;
    if (this.formInfo.value.templateType != CommunicationTemplateTypes.SMS) {
      const httpRequest = this.fileStorage.submit().pipe(
        switchMap((fileRefIds) => {
          var request = {
            electionId: this.election.id,
            subject: this.formContent.controls.subject.value,
            content: this.formContent.controls.content.value,
            fileReferenceIds: fileRefIds,
            templateType: this.formInfo.controls.templateType.value,
            teamIds: this.formRecipients.controls.selectedTeamIds.value ? this.formRecipients.controls.selectedTeamIds.value : [],
            workLocationIds: this.formRecipients.controls.selectedWorkLocationIds.value ? this.formRecipients.controls.selectedWorkLocationIds.value : [],
            taskTypeIds: this.formRecipients.controls.selectedTaskTypeIds.value ? this.formRecipients.controls.selectedTaskTypeIds.value : [],
            taskDates: this.formRecipients.controls.selectedDates.value ? this.formRecipients.controls.selectedDates.value : [],
            taskStatuses: this.formRecipients.controls.selectedTaskStatusIds.value ? this.formRecipients.controls.selectedTaskStatusIds.value : []
          };
  
        return this.communicationHttpService.sendGroupMessage(request);
        }),
      );
  
      this.subs.sink = event.pipe(httpRequest).subscribe((res) => {
        if (res.isSuccess) {
          this.sending = false;
          this.wizard.navigate();
        }
        else {
          this.sending = false;
        }
      });
    }
    else {
      var request = {
        electionId: this.election.id,
        content: this.formInfo.value.templateType == CommunicationTemplateTypes.SMS ? this.formContent.controls.plainTextContent.value : this.formContent.controls.content.value,
        fileReferenceIds: [],
        templateType: this.formInfo.controls.templateType.value,
        teamIds: this.formRecipients.controls.selectedTeamIds.value ? this.formRecipients.controls.selectedTeamIds.value : [],
        workLocationIds: this.formRecipients.controls.selectedWorkLocationIds.value ? this.formRecipients.controls.selectedWorkLocationIds.value : [],
        taskTypeIds: this.formRecipients.controls.selectedTaskTypeIds.value ? this.formRecipients.controls.selectedTaskTypeIds.value : [],
        taskDates: this.formRecipients.controls.selectedDates.value ? this.formRecipients.controls.selectedDates.value : [],
        taskStatuses: this.formRecipients.controls.selectedTaskStatusIds.value ? this.formRecipients.controls.selectedTaskStatusIds.value : []
      };
      this.communicationHttpService.sendGroupMessage(request).subscribe((res) => {
        if (res.isSuccess) {
          this.sending = false;
          this.wizard.navigate();
        }
        else {
          this.sending = false;
        }
      });
    }
  }

  onTemplateSelected(event) {
    this.subs.sink = this.communicationHttpService.getElectionCommunicationTemplate(event.value.id).subscribe((res) => {
      this.formContent.controls.subject.setValue(res.data.subject);
      this.formContent.controls.content.setValue(res.data.content);
      this.formContent.controls.plainTextContent.setValue(res.data.content);
      this.communicationTemplateFileReferences = res.data.communicationTemplateFileReferences;
    });
  }

  renderTeamNames(teamIds: string[]) {
    if (!teamIds) return undefined;

    const names = teamIds.map((id) => {
      const value = this.teams.find((i) => i.id == id);
      return value?.name ?? id;
    });

    return names.join(', ');
  }

  cancel() {
    this.wizard.navigate();
  }
}
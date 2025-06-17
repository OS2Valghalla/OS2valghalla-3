import { AfterViewInit, ChangeDetectorRef, Component, OnDestroy, OnInit, ViewChild, ViewEncapsulation } from '@angular/core';
import { SubSink } from 'subsink';
import { FormBuilder, Validators } from '@angular/forms';
import { FormPageComponent } from 'src/shared/components/form-page/form-page.component';
import { FormPageEvent } from 'src/shared/models/ux/form-page';
import { TaskTypeHttpService } from '../../services/task-type-http.service';
import { TaskTypeDetails } from '../../models/task-type-details';
import { CreateTaskTypeRequest } from '../../models/create-task-type-request';
import { UpdateTaskTypeRequest } from '../../models/update-task-type-request';
import { RichTextEditorOptions } from 'src/shared/models/ux/rich-text-editor-options';
import { FileStorageComponent } from 'src/shared/components/file-storage/file-storage.component';
import { switchMap } from 'rxjs';
import { WorkLocationInfo } from 'src/features/tasks/models/work-location-info';
import { WorkLocationDetails } from 'src/features/administration/work-location/models/work-location-details';
import { ElectionDetails } from 'src/features/administration/election/models/election-details';
import { ElectionHttpService } from 'src/features/administration/election/services/election-http.service';
import { WorkLocationHttpService } from 'src/features/administration/work-location/services/work-location-http.service';
import { TaskTypeTemplateDetails } from 'src/features/administration/task-type-template/models/task-type-template-details';
import { TaskTypeTemplateHttpService } from 'src/features/administration/task-type-template/services/task-type-template-http.service';

@Component({
  selector: 'app-admin-task-type-item',
  templateUrl: './task-type-item.component.html',
  styleUrls: ['./task-type-item.component.scss'],
  providers: [TaskTypeHttpService, ElectionHttpService, WorkLocationHttpService, TaskTypeTemplateHttpService],
  encapsulation: ViewEncapsulation.None
})
export class TaskTypeItemComponent implements OnInit, AfterViewInit, OnDestroy {
  private readonly subs = new SubSink();

  loading: boolean = true;
  item?: TaskTypeDetails;
  workLocations: Array<WorkLocationDetails> = [];
  elections: Array<ElectionDetails> = [];
  taskTypeTemplates: Array<TaskTypeTemplateDetails> = [];
  taskTypeTemplateIdBeforeEdit: string | null = null;
  workLocationIdBeforeEdit: string | null = null;
  electionIdBeforeEdit: string | null = null;


  readonly rtfOptions: RichTextEditorOptions = {
    isRichText: true,
    heightInPixel: 300,
    removeStyleWhenPasting: true,
  };

  readonly form = this.formBuilder.group({
    title: ['', Validators.required],
    shortName: ['', Validators.required],
    electionId: ['', Validators.required],
    workLocationId: ['', Validators.required],
    taskTypeTemplateId: ['', Validators.required],
    description: ['', Validators.required],
    startTime: [undefined as string, Validators.required],
    endTime: [undefined as string, Validators.required],
    payment: [undefined],
    validationNotRequired: [false],
    trusted: [false],
    sendingReminderEnabled: [true],
  });

  @ViewChild(FormPageComponent) readonly formPage: FormPageComponent;
  @ViewChild(FileStorageComponent) private readonly fileStorage: FileStorageComponent;

  constructor(
    private readonly formBuilder: FormBuilder,
    private readonly changeDetectorRef: ChangeDetectorRef,
    private readonly taskTypeHttpService: TaskTypeHttpService,
    private readonly electionHttpService: ElectionHttpService,
    private readonly workLocationHttpService: WorkLocationHttpService,
    private readonly taskTypeTemplateHttpService: TaskTypeTemplateHttpService
  ) { }

  ngOnInit(): void {
    this.form.addValidators((control) => {
      const startTime = control && control.get('startTime') && control.get('startTime').value;
      const endTime = control && control.get('endTime') && control.get('endTime').value;
      if (!startTime || !endTime) return null;

      if (endTime <= startTime) {
        return { invalidEndTime: true };
      }

      return null;
    });
  }

  ngAfterViewInit(): void {
    this.electionHttpService.getAllElections().subscribe((response) => {
      this.elections = response.data;
    });
    this.subs.sink = this.form.controls.electionId.valueChanges.subscribe((electionId) => {
      if (electionId) {
        this.workLocationHttpService.getAllWorkLocationsForElection(electionId).subscribe((res) => {
          this.workLocations = res.data;
          this.changeDetectorRef.detectChanges();
        });
      } else {
        this.workLocations = [];
      }
    });
    this.taskTypeTemplateHttpService.getAllTaskTypeTemplates().subscribe((response) => {
      this.taskTypeTemplates = response.data;
    });
    this.subs.sink = this.formPage.state$.subscribe(() => {
      if (!this.formPage.isUpdateForm()) {
        this.loading = false;
        this.changeDetectorRef.detectChanges();
        return;
      }

      this.subs.sink = this.taskTypeHttpService.getTaskTypeDetails(this.formPage.itemId).subscribe((res) => {
        this.loading = false;
        this.item = res.data;
        this.taskTypeTemplateIdBeforeEdit = res.data.taskTypeTemplateId;
        this.workLocationIdBeforeEdit = res.data.workLocationId;
        this.electionIdBeforeEdit = res.data.electionId;

        if (res.data) {
          this.form.setValue({
            title: res.data.title,
            shortName: res.data.shortName,
            description: res.data.description,
            startTime: res.data.startTime.substring(0, 5),
            endTime: res.data.endTime.substring(0, 5),
            payment: res.data.payment,
            validationNotRequired: res.data.validationNotRequired,
            trusted: res.data.trusted,
            sendingReminderEnabled: res.data.sendingReminderEnabled,
            electionId: res.data.electionId ?? '',
            workLocationId: res.data.workLocationId ?? '',
            taskTypeTemplateId: res.data.taskTypeTemplateId ?? ''
          });
          this.form.controls['electionId'].disable();
        }
      });
    });
  }

  ngOnDestroy(): void {
    this.subs.unsubscribe();
  }

  createTaskType(event: FormPageEvent) {
    const httpRequest = this.fileStorage.submit().pipe(
      switchMap((fileRefIds) => {
        const request: CreateTaskTypeRequest = {
          title: this.form.value.title,
          shortName: this.form.value.shortName,
          description: this.form.value.description,
          startTime: this.form.value.startTime + ':00',
          endTime: this.form.value.endTime + ':00',
          payment: this.form.value.payment,
          validationNotRequired: this.form.value.validationNotRequired,
          trusted: this.form.value.trusted,
          sendingReminderEnabled: this.form.value.sendingReminderEnabled,
          fileReferenceIds: fileRefIds,
          workLocationId: this.form.value.workLocationId,
          electionId: this.form.value.electionId,
          taskTypeTemplateId: this.form.value.taskTypeTemplateId
        };

        return this.taskTypeHttpService.createTaskType(request);
      }),
    );

    this.subs.sink = event.pipe(httpRequest).subscribe((res) => {
      if (res.isSuccess) {
        this.formPage.navigate();
      }
    });
  }

  updateTaskType(event: FormPageEvent) {
    const httpRequest = this.fileStorage.submit().pipe(
      switchMap((fileRefIds) => {
        const request: UpdateTaskTypeRequest = {
          id: this.formPage.itemId,
          title: this.form.value.title,
          shortName: this.form.value.shortName,
          description: this.form.value.description,
          startTime: this.form.value.startTime + ':00',
          endTime: this.form.value.endTime + ':00',
          payment: this.form.value.payment,
          validationNotRequired: this.form.value.validationNotRequired,
          trusted: this.form.value.trusted,
          sendingReminderEnabled: this.form.value.sendingReminderEnabled,
          fileReferenceIds: fileRefIds,
          newElectionId: this.form.value.electionId,
          electionId: this.electionIdBeforeEdit,
          newWorkLocationId: this.form.value.workLocationId,
          workLocationId: this.workLocationIdBeforeEdit,
          newTaskTypeTemplateId: this.form.value.taskTypeTemplateId,
          taskTypeTemplateId: this.taskTypeTemplateIdBeforeEdit
        };

        return this.taskTypeHttpService.updateTaskType(request);
      }),
    );

    this.subs.sink = event.pipe(httpRequest).subscribe((res) => {
      if (res.isSuccess) {
        this.formPage.navigate();
      }
    });
  }

  deleteTaskType(event: FormPageEvent) {
    const httpRequest = this.fileStorage.deleteAllFiles().pipe(
      switchMap(() => {
        return this.taskTypeHttpService.deleteTaskType(this.formPage.itemId);
      }),
    );

    this.subs.sink = event.pipe(httpRequest).subscribe((res) => {
      if (res.isSuccess) {
        this.formPage.navigate();
      }
    });
  }

  isEmptyContent() {
    if ((this.form.controls.description.touched && (!this.form.controls.description.value || this.form.controls.description.value == '<p></p>'))) {
      this.form.controls.description.setErrors({ required: true });
      return true;
    }

    return false;
  }

  cancel() {
    this.formPage.navigate();
  }

  setStartTime(event) {
    this.form.value.startTime = event.toString();
  }

  setEndTime(event) {
    this.form.value.endTime = event.toString();
  }
}

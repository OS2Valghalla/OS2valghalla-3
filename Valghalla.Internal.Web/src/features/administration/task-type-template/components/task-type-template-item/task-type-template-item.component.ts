import { AfterViewInit, ChangeDetectorRef, Component, OnDestroy, OnInit, ViewChild, ViewEncapsulation } from '@angular/core';
import { SubSink } from 'subsink';
import { FormBuilder, Validators } from '@angular/forms';
import { FormPageComponent } from 'src/shared/components/form-page/form-page.component';
import { FormPageEvent } from 'src/shared/models/ux/form-page';
import { TaskTypeTemplateHttpService } from '../../services/task-type-template-http.service';
import { TaskTypeTemplateDetails } from '../../models/task-type-template-details';
import { CreateTaskTypeTemplateRequest } from '../../models/create-task-type-template-request';
import { UpdateTaskTypeTemplateRequest } from '../../models/update-task-type-template-request';
import { RichTextEditorOptions } from 'src/shared/models/ux/rich-text-editor-options';
import { FileStorageComponent } from 'src/shared/components/file-storage/file-storage.component';
import { switchMap } from 'rxjs';

@Component({
  selector: 'app-admin-task-type-template-item',
  templateUrl: './task-type-template-item.component.html',
  styleUrls: ['./task-type-template-item.component.scss'],
  providers: [TaskTypeTemplateHttpService],
  encapsulation: ViewEncapsulation.None
})
export class TaskTypeTemplateItemComponent implements OnInit, AfterViewInit, OnDestroy {
  private readonly subs = new SubSink();

  loading: boolean = true;
  item?: TaskTypeTemplateDetails;

  readonly rtfOptions: RichTextEditorOptions = {
    isRichText: true,
    heightInPixel: 300,
    removeStyleWhenPasting: true,
  };

  readonly form = this.formBuilder.group({
    title: ['', Validators.required],
    shortName: ['', Validators.required],
    description: ['', Validators.required],
    startTime: [undefined as string, Validators.required],
    endTime: [undefined as string, Validators.required],
    payment: [0],
    validationNotRequired: [false],
    trusted: [false],
    sendingReminderEnabled: [true],
  });

  @ViewChild(FormPageComponent) private readonly formPage: FormPageComponent;
  @ViewChild(FileStorageComponent) private readonly fileStorage: FileStorageComponent;

  constructor(
    private readonly formBuilder: FormBuilder,
    private readonly changeDetectorRef: ChangeDetectorRef,
    private readonly taskTypeTemplateHttpService: TaskTypeTemplateHttpService,
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
    this.subs.sink = this.formPage.state$.subscribe(() => {
      if (!this.formPage.isUpdateForm()) {
        this.loading = false;
        this.changeDetectorRef.detectChanges();
        return;
      }

      this.subs.sink = this.taskTypeTemplateHttpService.getTaskTypeTemplateDetails(this.formPage.itemId).subscribe((res) => {
        this.loading = false;
        this.item = res.data;

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
          });
        }
      });
    });
  }

  ngOnDestroy(): void {
    this.subs.unsubscribe();
  }

  createTaskTypeTemplate(event: FormPageEvent) {
    const httpRequest = this.fileStorage.submit().pipe(
      switchMap((fileRefIds) => {
        const request: CreateTaskTypeTemplateRequest = {
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
        };

        return this.taskTypeTemplateHttpService.createTaskTypeTemplate(request);
      }),
    );

    this.subs.sink = event.pipe(httpRequest).subscribe((res) => {
      if (res.isSuccess) {
        this.formPage.navigate();
      }
    });
  }

  updateTaskTypeTemplate(event: FormPageEvent) {
    const httpRequest = this.fileStorage.submit().pipe(
      switchMap((fileRefIds) => {
        const request: UpdateTaskTypeTemplateRequest = {
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
        };

        return this.taskTypeTemplateHttpService.updateTaskTypeTemplate(request);
      }),
    );

    this.subs.sink = event.pipe(httpRequest).subscribe((res) => {
      if (res.isSuccess) {
        this.formPage.navigate();
      }
    });
  }

  deleteTaskTypeTemplate(event: FormPageEvent) {
    const httpRequest = this.fileStorage.deleteAllFiles().pipe(
      switchMap(() => {
        return this.taskTypeTemplateHttpService.deleteTaskTypeTemplate(this.formPage.itemId);
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

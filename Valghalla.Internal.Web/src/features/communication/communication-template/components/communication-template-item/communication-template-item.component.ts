import { AfterViewInit, ChangeDetectorRef, Component, OnDestroy, ViewChild } from '@angular/core';
import { SubSink } from 'subsink';
import { switchMap } from 'rxjs';
import { FormBuilder, Validators } from '@angular/forms';
import { StepperSelectionEvent } from '@angular/cdk/stepper';
import { TranslocoService } from '@ngneat/transloco';
import { RichTextEditorOptions } from 'src/shared/models/ux/rich-text-editor-options';
import { WizardComponent } from 'src/shared/components/wizard/wizard.component';
import { FileStorageComponent } from 'src/shared/components/file-storage/file-storage.component';
import { FormPageEvent } from 'src/shared/models/ux/form-page';
import { CommunicationTemplateTypes } from 'src/shared/constants/communication-template-types';
import { CommunicationTemplateDetails } from '../../models/communication-template-details';
import { CreateCommunicationTemplateRequest } from '../../models/create-communication-template-request';
import { UpdateCommunicationTemplateRequest } from '../../models/update-communication-template-request';
import { CommunicationHttpService } from '../../services/communication-http.service';

@Component({
  selector: 'app-communication-template-item',
  templateUrl: './communication-template-item.component.html',
  styleUrls: ['../../../../../shared/components/table/table.component.scss'],
  providers: [CommunicationHttpService],
})
export class CommunicationTemplateItemComponent implements AfterViewInit, OnDestroy {
  private readonly subs = new SubSink();

  loading: boolean = false;

  item: CommunicationTemplateDetails;

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
    title: ['', Validators.required],
    templateType: [CommunicationTemplateTypes.DigitalPost, Validators.required],
  });

  readonly formContent = this.formBuilder.group({
    subject: ['', [Validators.required, Validators.maxLength(256)]],
    content: ['', Validators.required],
    plainTextContent: [''],
  });

  previewSubject: string = '';

  previewContent: string = '';

  preparingPreviewContent = false;

  @ViewChild(WizardComponent) readonly wizard: WizardComponent;
  @ViewChild(FileStorageComponent) private readonly fileStorage: FileStorageComponent;

  constructor(
    private readonly changeDetectorRef: ChangeDetectorRef,
    private readonly translocoService: TranslocoService,
    private readonly formBuilder: FormBuilder,
    private readonly communicationHttpService: CommunicationHttpService,
  ) {}
  
  ngAfterViewInit(): void {
    this.subs.sink = this.wizard.state$.subscribe(() => {
        if (!this.wizard.isUpdateWizard()) {
          this.loading = false;
          this.changeDetectorRef.detectChanges();
          return;
        }
  
        this.subs.sink = this.communicationHttpService
          .getElectionCommunicationTemplate(this.wizard.itemId)
          .subscribe((res) => {
            this.loading = false;
            this.item = res.data;
  
            if (res.data) {
              this.formInfo.setValue({
                title: res.data.title,
                templateType: res.data.templateType,
              });
              this.formContent.setValue({
                subject: res.data.subject,
                content: res.data.content,
                plainTextContent: res.data.content,
              });
              this.templateTypeChanged();
              this.formContent.markAsDirty();
            }
          });
      });
  }

  ngOnDestroy(): void {
    this.subs.unsubscribe();
  }

  templateTypeChanged() {   
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

  createCommunicationTemplate(event: FormPageEvent) {
    if (this.formInfo.value.templateType != CommunicationTemplateTypes.SMS){
        const httpRequest = this.fileStorage.submit().pipe(
            switchMap((fileRefIds) => {
            const createRequest: CreateCommunicationTemplateRequest = {
                title: this.formInfo.value.title,
                templateType: this.formInfo.value.templateType,
                subject: this.formContent.value.subject,
                content: this.formContent.value.content,
                fileReferenceIds: fileRefIds,
            };

            return this.communicationHttpService.createCommunicationTemplate(createRequest);
            }),
        );

        this.subs.sink = event.pipe(httpRequest).subscribe((res) => {
          if (res.isSuccess) {
              this.wizard.navigate();
          }
        });
    }
    else {
        const createRequestSMS: CreateCommunicationTemplateRequest = {
            title: this.formInfo.value.title,
            templateType: this.formInfo.value.templateType,
            content: this.formContent.value.plainTextContent
        };
    
        this.subs.sink = event.pipe(this.communicationHttpService.createCommunicationTemplate(createRequestSMS)).subscribe((res) => {
            if (res.isSuccess) {
                this.wizard.navigate();
            }
        });
    }
  }

  updateCommunicationTemplate(event: FormPageEvent) {
    if (this.formInfo.value.templateType != CommunicationTemplateTypes.SMS){
      const httpRequest = this.fileStorage.submit().pipe(
          switchMap((fileRefIds) => {
          const updateRequest: UpdateCommunicationTemplateRequest = {
              id: this.wizard.itemId,
              title: this.formInfo.value.title,
              templateType: this.formInfo.value.templateType,
              subject: this.formContent.value.subject,
              content: this.formContent.value.content,
              fileReferenceIds: fileRefIds,
          };

          return this.communicationHttpService.updateCommunicationTemplate(updateRequest);
          }),
      );

      this.subs.sink = event.pipe(httpRequest).subscribe((res) => {
      if (res.isSuccess) {
          this.wizard.navigate();
      }
      });
  }
  else {
      const updateRequestSMS: UpdateCommunicationTemplateRequest = {
          id: this.wizard.itemId,
          title: this.formInfo.value.title,
          templateType: this.formInfo.value.templateType,
          content: this.formContent.value.plainTextContent
      };
  
      this.subs.sink = event.pipe(this.communicationHttpService.updateCommunicationTemplate(updateRequestSMS)).subscribe((res) => {
          if (res.isSuccess) {
              this.wizard.navigate();
          }
      });
    }
  }

  deleteCommunicationTemplate(event: FormPageEvent) {
    this.subs.sink = event
      .pipe(this.communicationHttpService.deleteCommunicationTemplate(this.wizard.itemId))
      .subscribe((res) => {
        if (res.isSuccess) {
          this.wizard.navigate();
        }
      });
  }

  cancel() {
    this.wizard.navigate();
  }

  isEmptyContent() {
    if ((this.formInfo.value.templateType != CommunicationTemplateTypes.SMS && this.formContent.controls.content.touched && (!this.formContent.controls.content.value || this.formContent.controls.content.value == '<p></p>')) || (this.formInfo.value.templateType == CommunicationTemplateTypes.SMS && this.formContent.controls.plainTextContent.touched && !this.formContent.controls.plainTextContent.value)) return true;

    return false;
  }

  onStepEnter(event: StepperSelectionEvent) {
    if (event.selectedIndex == 2) {
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
}
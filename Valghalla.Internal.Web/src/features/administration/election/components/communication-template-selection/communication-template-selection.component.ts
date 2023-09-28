import { Component, Input, Output, OnInit, EventEmitter } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { CommunicationTemplateTypes } from 'src/shared/constants/communication-template-types';
import { CommunicationTemplateShared } from 'src/shared/models/communication/communication-template-shared';

@Component({
  selector: 'app-communication-template-selection',
  templateUrl: './communication-template-selection.component.html'
})
export class CommunicationTemplateSelectionComponent implements OnInit {
  @Input() label: string;

  @Input() hint?: string;

  @Input() communicationTemplates: CommunicationTemplateShared[];

  @Input() formGroup: FormGroup;

  @Input() inputControlName?: string;

  @Input() value?: CommunicationTemplateShared;

  @Input() selectedTemplateType?: CommunicationTemplateTypes;

  @Input() showStandardOption?: boolean;

  @Output() valueChange: EventEmitter<CommunicationTemplateShared> = new EventEmitter<CommunicationTemplateShared>();

  templateTypes: Array<any> = [];

  communicationTemplates_DigitalPost: CommunicationTemplateShared[] = [];
  communicationTemplates_Email: CommunicationTemplateShared[] = [];
  communicationTemplates_SMS: CommunicationTemplateShared[] = [];

  ngOnInit() {
    this.templateTypes = this.showStandardOption ? [
        { id: CommunicationTemplateTypes.Standard, title: 'communication.template_type.standard' },
        { id: CommunicationTemplateTypes.Email, title: 'communication.template_type.email' },
        { id: CommunicationTemplateTypes.DigitalPost, title: 'communication.template_type.digital_post' },    
        { id: CommunicationTemplateTypes.SMS, title: 'communication.template_type.sms' },
      ] : [
        { id: CommunicationTemplateTypes.Email, title: 'communication.template_type.email' },
        { id: CommunicationTemplateTypes.DigitalPost, title: 'communication.template_type.digital_post' },    
        { id: CommunicationTemplateTypes.SMS, title: 'communication.template_type.sms' },
      ];

    this.communicationTemplates_DigitalPost = this.communicationTemplates.filter(t => t.templateType == CommunicationTemplateTypes.DigitalPost);
    this.communicationTemplates_Email = this.communicationTemplates.filter(t => t.templateType == CommunicationTemplateTypes.Email);
    this.communicationTemplates_SMS = this.communicationTemplates.filter(t => t.templateType == CommunicationTemplateTypes.SMS);
    if (this.selectedTemplateType == null) {
        if (this.showStandardOption) {
            this.selectedTemplateType = CommunicationTemplateTypes.Standard;
        } else {
            this.selectedTemplateType = CommunicationTemplateTypes.DigitalPost;
        }
    }
  }

  onTemplateTypeChanged(event) {
    if (this.inputControlName) {
        this.formGroup.controls[this.inputControlName].setValue(null);
    } else {
        this.value = null;
        this.valueChange.emit(this.value);
        this.formGroup.markAsDirty();
    }
  }

  onTemplateChanged(event) {
    this.value = event.value;
    this.valueChange.emit(this.value);
    this.formGroup.markAsDirty();
  }
}

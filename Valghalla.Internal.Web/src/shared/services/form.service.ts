import { Injectable } from '@angular/core';
import { FormBuilder, FormControl, UntypedFormGroup } from '@angular/forms';
import { FormContainer } from '../models/form/form-container';
import { FormFieldDataReqest } from '../models/form/form-request';
import { FormTypes } from '../models/form/form-types';
@Injectable({
  providedIn: 'root',
})
export class FormService {
  formTypes = new FormTypes();
  constructor(private formBuilder: FormBuilder) {}

  // generateFormContainerWithValues(form: Form, fields: Fields[], details: FormPostDetails): FormContainer {
  //   const formContainer = new FormContainer();
  //   formContainer.title = form.name;
  //   formContainer.formGroup = new UntypedFormGroup({});
  //   const formFields = form.formFields
  //     .sort((a, b) => a.rowIndex - b.rowIndex)
  //     .map((formField) => {
  //       const field = fields.find((i) => i.id == formField.fieldId);
  //       const fieldData = details.fieldDataCollection.find((i) => i.fieldId == formField.fieldId);

  //       return {
  //         id: field.id,
  //         field: field,
  //         order: formField.rowIndex,
  //         value: fieldData ? fieldData.data : field.default,
  //       } as FormFields;
  //     });

  //   formFields.forEach((formField) => {
  //     const control = this.formBuilder.control(formField.value);
  //     const id = Math.random().toString();
  //     formContainer.formGroup.addControl(id, control);
  //     formContainer.questions.push({
  //       id: id,
  //       formField: formField,
  //     });
  //   });

  //   return formContainer;
  // }

  generateFormContainer(formResponse: any): FormContainer {
    let formContainer = new FormContainer();
    formContainer.title = formResponse.form.name;
    formContainer.readonly = false;
    formContainer.formGroup = new UntypedFormGroup({});

    formResponse.form.formFields
      .sort((x, y) => x.order - y.order)
      .map((formField) => {
        if (formField.field.type === this.formTypes.multiple_select) {
          const group = this.formBuilder.group({});
          formField.field.alternatives.forEach((alternative) => {
            let alternativeControl: FormControl;
            if (alternative.name === formField.field.value) {
              alternativeControl = this.formBuilder.control(true);
            } else {
              alternativeControl = this.formBuilder.control(false);
            }
            group.addControl(alternative.name, alternativeControl);
          });
          formContainer.formGroup.addControl(formField.field.id, group);
        } else {
          const control = this.formBuilder.control(formField.field.value ? formField.field.value : '');
          formContainer.formGroup.addControl(formField.field.id, control);
        }
        formContainer.questions.push({
          id: formField.field.id,
          formField: formField,
        });
      });

    return formContainer;
  }

  genereateFormFieldDataRequestFromForm(formContainer: FormContainer): FormFieldDataReqest[] {
    let fieldDataCollection: FormFieldDataReqest[] = [];
    formContainer.questions.forEach((item) => {
      let value = formContainer.formGroup.controls[item.id].value;
      const fieldId = item.formField.field.id;

      if (item.formField.field.type === this.formTypes.single_select) {
        value = Object.keys(value).map((key) => value[key])[0];
      } else if (item.formField.field.type === this.formTypes.multiple_select) {
        const answers = Object.keys(value)
          .filter((x) => value[x])
          .map((key) => {
            return {
              fieldId: fieldId,
              data: key,
            } as FormFieldDataReqest;
          });
        fieldDataCollection = [...fieldDataCollection, ...answers];
        return;
      }

      fieldDataCollection.push({
        fieldId: fieldId,
        data: value,
      } as FormFieldDataReqest);
    });
    return fieldDataCollection;
  }
}

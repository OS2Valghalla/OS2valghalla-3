import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { SubSink } from 'subsink';
import { switchMap } from 'rxjs';
import { FormBuilder, Validators, AbstractControl, ValidationErrors } from '@angular/forms';
import { FormPageEvent } from 'src/shared/models/ux/form-page';
import { FormPageComponent } from 'src/shared/components/form-page/form-page.component';
import { FileReference } from "src/shared/models/file-storage/file-reference";
import { WebPageHttpService } from '../services/web-page-http.service';
import { UpdateContactInformationRequest } from '../models/update-contact-information-request';
import { FileStorageComponent } from 'src/shared/components/file-storage/file-storage.component';

@Component({
  selector: 'app-web-contact-information',
  templateUrl: './contact-information.component.html',
  providers: [WebPageHttpService]
})
export class WebContactInformationComponent implements OnInit, OnDestroy {
    private subs = new SubSink();

    @ViewChild(FormPageComponent) private readonly formPage: FormPageComponent;
    @ViewChild(FileStorageComponent) private readonly fileStorage: FileStorageComponent;

    constructor(
      private formBuilder: FormBuilder,
      private webPageHttpService: WebPageHttpService
    ) {}

    loading = true;

    submitting = false;

    logoFileReferences: FileReference[];

    readonly form = this.formBuilder.group({
        municipalityName: ['', Validators.required],
        electionResponsibleApartment: ['', Validators.required],
        address: ['', Validators.required],
        postalCode: ['', [Validators.required, Validators.pattern(/^\d{4}$/)]],
        city: ['', Validators.required],
        telephoneNumber: ['', [Validators.pattern(/^\d{8,}$/)]],
        digitalPost: ['', [Validators.pattern(/^(?:http(s)?:\/\/)?[\w.-]+(?:\.[\w\.-]+)+[\w\-\._~:/?#[\]@!\$&'\(\)\*\+,;=.]+$/)]],
        email: ['', [Validators.pattern('^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,4}$')]],
        logoFileReferenceId: [''],
    });

    ngOnInit(): void {
        this.form.addValidators([this.validateEmptyEmailAndDigitalPost]);
        this.subs.sink = this.webPageHttpService.getContactInformation().subscribe({
            next: (res) => {
                if (res.isSuccess) {
                    var contactInformation = res.data;
                    this.form.setValue({
                        municipalityName: contactInformation.municipalityName,
                        electionResponsibleApartment: contactInformation.electionResponsibleApartment,
                        address: contactInformation.address,
                        postalCode: contactInformation.postalCode,
                        city: contactInformation.city,
                        telephoneNumber: contactInformation.telephoneNumber,
                        digitalPost: contactInformation.digitalPost,
                        email: contactInformation.email,
                        logoFileReferenceId: contactInformation.logoFileReferenceId
                    });
                    if (contactInformation.logoFileReference) {
                      this.logoFileReferences = [contactInformation.logoFileReference];
                    }
                    this.form.markAsDirty();
                }
                this.loading = false;
            },
            error: (_) => {
                console.error(_);
            },
        });
    }

    ngOnDestroy(): void {
        this.subs.unsubscribe();
    }

    saveWebPage(event: FormPageEvent) {
      const httpRequest = this.fileStorage.submit().pipe(
        switchMap((fileRefIds) => {
          const request: UpdateContactInformationRequest = {
            municipalityName: this.form.value.municipalityName,
            electionResponsibleApartment: this.form.value.electionResponsibleApartment,
            address: this.form.value.address,
            postalCode: this.form.value.postalCode,
            city: this.form.value.city,
            telephoneNumber: this.form.value.telephoneNumber,
            digitalPost: this.form.value.digitalPost,
            email: this.form.value.email,
            logoFileReferenceId: fileRefIds && fileRefIds.length > 0 ? fileRefIds[0] : null,
          };
  
          return this.webPageHttpService.updateContactInformation(request);
        }),
      );

      this.subs.sink = event.pipe(httpRequest).subscribe((res) => {
        if (res.isSuccess) {
          this.formPage.navigate();
        }
      });
    }

    isEmptyEmailAndDigitalPost() {
        if ((this.form.controls.digitalPost.touched || this.form.controls.email.touched) && !this.form.controls.digitalPost.value && !this.form.controls.email.value) return true;

        return false;
    }

    validateEmptyEmailAndDigitalPost(control: AbstractControl): ValidationErrors | null {
        const digitalPost = control && control.get('digitalPost') && control.get('digitalPost').value;
        const email = control && control.get('email') && control.get('email').value;
        if (!digitalPost && !email) {
          return { emptyEmailAndDigitalPost: true };
        }
    
        return null;
    }

    onCancel() {
        this.formPage.navigate();
    }
}

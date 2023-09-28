import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { SubSink } from 'subsink';
import { FormBuilder } from '@angular/forms';
import { FormPageEvent } from 'src/shared/models/ux/form-page';
import { FormPageComponent } from 'src/shared/components/form-page/form-page.component';
import { WebPageHttpService } from '../services/web-page-http.service';
import { UpdateDeclarationOfConsentPageRequest } from '../models/update-declaration-of-consent-page-request';

@Component({
  selector: 'app-web-declaration-of-consent-page',
  templateUrl: './declaration-of-consent-page.component.html',
  providers: [WebPageHttpService],
})
export class WebDeclarationOfConsentComponent implements OnInit, OnDestroy {
  private subs = new SubSink();

  @ViewChild(FormPageComponent) private readonly formPage: FormPageComponent;

  constructor(
    private formBuilder: FormBuilder,
    private webPageHttpService: WebPageHttpService,
  ) {}

  loading = true;

  submitting = false;

  readonly form = this.formBuilder.group({
    pageContent: [''],
    isActivated: false
  });

  ngOnInit(): void {
    this.subs.sink = this.webPageHttpService.getDeclarationOfConsentPage().subscribe({
      next: (res) => {
        if (res.isSuccess) {
          this.form.setValue({
            pageContent: res.data.pageContent,
            isActivated: res.data.isActivated
          });
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
    const request: UpdateDeclarationOfConsentPageRequest = {
      pageContent: this.form.value.pageContent,
      isActivated: this.form.value.isActivated
    };

    this.subs.sink = event.pipe(this.webPageHttpService.updateDeclarationOfConsentPage(request)).subscribe((res) => {
      if (res.isSuccess) {
        this.formPage.navigate();
      }
    });
  }

  cancel() {
    this.formPage.navigate();
  }
}

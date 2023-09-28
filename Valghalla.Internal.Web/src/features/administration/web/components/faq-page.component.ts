import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { SubSink } from 'subsink';
import { FormBuilder, Validators } from '@angular/forms';
import { RichTextEditorOptions } from 'src/shared/models/ux/rich-text-editor-options';
import { FormPageEvent } from 'src/shared/models/ux/form-page';
import { FormPageComponent } from 'src/shared/components/form-page/form-page.component';
import { WebPageHttpService } from '../services/web-page-http.service';
import { UpdateFAQPageRequest } from '../models/update-faq-page-request';

@Component({
  selector: 'app-web-faq-page',
  templateUrl: './faq-page.component.html',
  providers: [WebPageHttpService],
})
export class WebFAQPageComponent implements OnInit, OnDestroy {
  private subs = new SubSink();

  @ViewChild(FormPageComponent) private readonly formPage: FormPageComponent;

  constructor(
    private formBuilder: FormBuilder,
    private webPageHttpService: WebPageHttpService,
  ) {}

  loading = true;

  submitting = false;

  readonly pageContentSettings: RichTextEditorOptions = {
    fieldTitle: 'administration.web.faq.content',
    isRichText: true,
    heightInPixel: 300,
    removeStyleWhenPasting: true,
  };

  readonly form = this.formBuilder.group({
    pageContent: [''],
    isActivated: false
  });

  ngOnInit(): void {
    this.subs.sink = this.webPageHttpService.getFAQPage().subscribe({
      next: (res) => {
        if (res.isSuccess) {
          this.form.setValue({
            pageContent: res.data.pageContent,
            isActivated: res.data.isActivated,
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
    const request: UpdateFAQPageRequest = {
      pageContent: this.form.value.pageContent,
      isActivated: this.form.value.isActivated
    };

    this.subs.sink = event.pipe(this.webPageHttpService.updateFAQPage(request)).subscribe((res) => {
      if (res.isSuccess) {
        this.formPage.navigate();
      }
    });
  }

  cancel() {
    this.formPage.navigate();
  }
}

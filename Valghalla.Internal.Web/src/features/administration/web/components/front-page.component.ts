import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { SubSink } from 'subsink';
import { FormBuilder } from '@angular/forms';
import { FormPageEvent } from 'src/shared/models/ux/form-page';
import { FormPageComponent } from 'src/shared/components/form-page/form-page.component';
import { RichTextEditorOptions } from 'src/shared/models/ux/rich-text-editor-options';
import { WebPageHttpService } from '../services/web-page-http.service';
import { UpdateFrontPageRequest } from '../models/update-front-page-request';

@Component({
  selector: 'app-web-front-page',
  templateUrl: './front-page.component.html',
  providers: [WebPageHttpService],
})
export class WebFrontPageComponent implements OnInit, OnDestroy {
  private subs = new SubSink();

  @ViewChild(FormPageComponent) private readonly formPage: FormPageComponent;

  constructor(
    private formBuilder: FormBuilder,
    private webPageHttpService: WebPageHttpService,
  ) {}

  loading = true;

  submitting = false;

  readonly titleSettings: RichTextEditorOptions = {
    fieldTitle: 'administration.web.front.title',
    isRichText: false,
    removeStyleWhenPasting: true,
  };

  readonly pageContentSettings: RichTextEditorOptions = {
    fieldTitle: 'administration.web.front.content',
    isRichText: true,
    heightInPixel: 300,
    removeStyleWhenPasting: true,
  };

  readonly form = this.formBuilder.group({
    pageContent: [''],
    title: [''],
  });

  ngOnInit(): void {
    this.subs.sink = this.webPageHttpService.getFrontPage().subscribe({
      next: (res) => {
        if (res.isSuccess) {
          this.form.setValue({
            pageContent: res.data.pageContent,
            title: res.data.title
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
    const request: UpdateFrontPageRequest = {
      pageContent: this.form.value.pageContent,
      title: this.form.value.title
    };

    this.subs.sink = event.pipe(this.webPageHttpService.updateFrontPage(request)).subscribe((res) => {
      if (res.isSuccess) {
        this.formPage.navigate();
      }
    });
  }

  cancel() {
    this.formPage.navigate();
  }
}

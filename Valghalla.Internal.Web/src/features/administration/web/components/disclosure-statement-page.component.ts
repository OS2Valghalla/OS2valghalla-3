import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { SubSink } from 'subsink';
import { FormBuilder, Validators } from '@angular/forms';
import { RichTextEditorOptions } from 'src/shared/models/ux/rich-text-editor-options';
import { FormPageEvent } from 'src/shared/models/ux/form-page';
import { FormPageComponent } from 'src/shared/components/form-page/form-page.component';
import { WebPageHttpService } from '../services/web-page-http.service';
import { UpdateWebPageRequest } from '../models/update-web-page-request';

@Component({
  selector: 'app-web-diclosure-statement-page',
  templateUrl: './disclosure-statement-page.component.html',
  providers: [WebPageHttpService],
})
export class WebDisclosureStatementComponent implements OnInit, OnDestroy {
  private subs = new SubSink();

  @ViewChild(FormPageComponent) private readonly formPage: FormPageComponent;

  constructor(
    private formBuilder: FormBuilder,
    private webPageHttpService: WebPageHttpService,
  ) {}

  loading = true;

  submitting = false;

  readonly pageContentSettings: RichTextEditorOptions = {
    fieldTitle: 'administration.web.disclosure_statement.content',
    isRichText: true,
    heightInPixel: 300,
    removeStyleWhenPasting: true,
  };

  readonly form = this.formBuilder.group({
    pageContent: [''],
  });

  ngOnInit(): void {
    this.subs.sink = this.webPageHttpService.getDisclosureStatementPage().subscribe({
      next: (res) => {
        if (res.isSuccess) {
          this.form.setValue({
            pageContent: res.data.pageContent,
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
    const request: UpdateWebPageRequest = {
      pageContent: this.form.value.pageContent,
    };

    this.subs.sink = event.pipe(this.webPageHttpService.updateDisclosureStatementPage(request)).subscribe((res) => {
      if (res.isSuccess) {
        this.formPage.navigate();
      }
    });
  }

  cancel() {
    this.formPage.navigate();
  }
}

import { Component, AfterViewInit } from '@angular/core';
import { SubSink } from 'subsink';
import { WebPage } from 'src/shared/models/web-page';
import { UnprotectedWebHttpService } from 'src/shared/services/unprotected-web-http.service';

@Component({
  selector: 'app-faq-landing',
  templateUrl: './landing.component.html',
  providers: [UnprotectedWebHttpService]
})
export class FaqLandingComponent implements AfterViewInit {
  private readonly subs = new SubSink();

  loading = true;

  page: WebPage;

  constructor(
    private unprotectedWebHttpService: UnprotectedWebHttpService,    
  ) {}

  ngAfterViewInit() {
    this.subs.sink = this.unprotectedWebHttpService.getFAQPage().subscribe((res) => {
      if (res.data) {
        this.page = res.data;
      }
      this.loading = false;
    });
  }

}

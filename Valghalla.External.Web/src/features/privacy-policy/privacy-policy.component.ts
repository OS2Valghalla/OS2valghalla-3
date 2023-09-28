import { Component } from '@angular/core';
import { SubSink } from 'subsink';
import { WebPage } from 'src/shared/models/web-page';
import { WebHttpService } from 'src/shared/services/web-http.service';

@Component({
  selector: 'app-privacy-policy',
  templateUrl: './privacy-policy.component.html',
})
export class PrivacyPolciyComponent {
  private readonly subs = new SubSink();

  page: WebPage;

  loading = true;

  constructor(private readonly webHttpService: WebHttpService) {}

  ngOnInit() {
    this.subs.sink = this.webHttpService.getDisclosureStatementPage().subscribe((res) => {
      if (res.isSuccess) {
        this.page = res.data;
      }
      this.loading = false;
    });
  }
}

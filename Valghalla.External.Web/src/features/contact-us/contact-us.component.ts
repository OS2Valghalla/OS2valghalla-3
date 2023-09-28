import { Component, OnDestroy, OnInit } from '@angular/core';
import { SubSink } from 'subsink';
import { GlobalStateService } from 'src/app/global-state.service';
import { ElectionCommitteeContactInformationPage } from 'src/shared/models/web-page';

@Component({
  selector: 'app-contact-us',
  templateUrl: './contact-us.component.html',
})
export class ContactUsComponent implements OnInit, OnDestroy {
  private readonly subs = new SubSink();

  webPage: ElectionCommitteeContactInformationPage;

  constructor(private readonly globalStateService: GlobalStateService) {}

  ngOnInit() {
    this.getGlobalData();
  }

  ngOnDestroy(): void {
    this.subs.unsubscribe();
  }

  getGlobalData() {
    this.subs.sink = this.globalStateService.webPage$.subscribe((webPage) => {
      this.webPage = webPage;
    });
  }
}

import { Component, EventEmitter, OnDestroy, OnInit, Output } from '@angular/core';
import { WebPage } from 'src/shared/models/web-page';
import { WebHttpService } from 'src/shared/services/web-http.service';
import { SubSink } from 'subsink';

@Component({
  selector: 'app-disclosure-statement-confirmator',
  templateUrl: './disclosure-statement-confirmator.component.html',
})
export class DisclosureStatementConfirmatorComponent implements OnInit, OnDestroy {
  private readonly subs = new SubSink();

  @Output() next = new EventEmitter<void>();
  @Output() cancel = new EventEmitter<void>();

  loading: boolean = true;
  page?: WebPage;

  constructor(private readonly webHttpService: WebHttpService) {}

  ngOnInit(): void {
    this.subs.sink = this.webHttpService.getDisclosureStatementPage().subscribe((res) => {
      this.loading = false;
      this.page = res.data;
    });
  }

  ngOnDestroy(): void {
    this.subs.unsubscribe();
  }

  continue() {
    this.next.emit();
  }

  cancelRegistration() {
    this.cancel.emit();
  }
}

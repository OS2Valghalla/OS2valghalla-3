import { Component, AfterViewInit } from '@angular/core';
import { SubSink } from 'subsink';
import { take } from 'rxjs';
import { WebPage } from 'src/shared/models/web-page';
import { UnprotectedWebHttpService } from 'src/shared/services/unprotected-web-http.service';
import { AuthService } from '../../../app/auth/auth.service';
import { GlobalStateService } from '../../../app/global-state.service';

@Component({
  selector: 'app-unprotected-landing',
  templateUrl: './landing.component.html',
  providers: [UnprotectedWebHttpService],
})
export class UnprotectedLandingComponent implements AfterViewInit {
  private readonly subs = new SubSink();

  loading = true;

  electionActivated: boolean = false;

  page: WebPage;

  constructor(
    private readonly unprotectedWebHttpService: UnprotectedWebHttpService,
    private readonly authService: AuthService,
    private readonly globalStateService: GlobalStateService
  ) {}

  ngAfterViewInit() {
    this.globalStateService.electionActivated$.pipe(take(1)).subscribe((electionActivated) => {
      this.electionActivated = electionActivated;
      if (electionActivated) {
        this.subs.sink = this.unprotectedWebHttpService.getFrontPage().subscribe((res) => {
          if (res.data) {
            this.page = res.data;
          }
          this.loading = false;
        });
      }
      else {
        this.loading = false;
      }
    });
   
  }

  get signedIn() {
    return this.authService.isSessionExist();
  }

  login() {
    this.authService.login();
  }
}

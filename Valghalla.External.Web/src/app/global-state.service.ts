import { Injectable } from '@angular/core';
import { ReplaySubject, take } from 'rxjs';
import { AppHttpService } from './app-http.service';
import { User } from 'src/app/models/user';
import { ElectionCommitteeContactInformationPage } from 'src/shared/models/web-page';
import { AuthService } from './auth/auth.service';

@Injectable({
  providedIn: 'root',
})
export class GlobalStateService {
  private readonly user = new ReplaySubject<User>(1);
  readonly user$ = this.user.asObservable();

  private readonly webPage = new ReplaySubject<ElectionCommitteeContactInformationPage>(1);
  readonly webPage$ = this.webPage.asObservable();

  private readonly electionActivated = new ReplaySubject<boolean>(1);
  readonly electionActivated$ = this.electionActivated.asObservable();

  private readonly faqPageActivated = new ReplaySubject<boolean>(1);
  readonly faqPageActivated$ = this.faqPageActivated.asObservable();

  readonly userTeamFetching = new ReplaySubject<void>(1);
  readonly userTeamFetching$ = this.userTeamFetching.asObservable();

  constructor(private appHttpService: AppHttpService, private authService: AuthService) {
    this.appHttpService
      .getAppContext()
      .pipe(take(1))
      .subscribe((res) => {
        this.electionActivated.next(res.data.electionActivated);
        this.faqPageActivated.next(res.data.faqPageActivated);
        this.webPage.next(res.data.webPage);
      });

      this.authService.authorized$.pipe(take(1)).subscribe((authorized) => {
        if (!authorized) return;

        this.appHttpService.getUser().pipe(take(1)).subscribe((res) => {
          this.user.next(res.data);
        });
      })
  }
}

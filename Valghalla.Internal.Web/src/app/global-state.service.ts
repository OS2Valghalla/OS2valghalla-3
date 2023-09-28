import { Injectable } from '@angular/core';
import { ReplaySubject, take } from 'rxjs';
import { AppHttpService } from './app-http.service';
import { User } from 'src/app/models/user';
import { SELECTED_ELECTION_TO_WORK_ON } from 'src/shared/constants/election';
import { AppElection } from './models/app-election';

@Injectable({
  providedIn: 'root',
})
export class GlobalStateService {
  private readonly user = new ReplaySubject<User>(1);
  readonly user$ = this.user.asObservable();

  private readonly election = new ReplaySubject<AppElection>(1);
  readonly election$ = this.election.asObservable();

  private readonly municipalityName = new ReplaySubject<string>(1);
  readonly municipalityName$ = this.municipalityName.asObservable();

  private readonly externalWebUrl = new ReplaySubject<string>(1);
  readonly externalWebUrl$ = this.externalWebUrl.asObservable();

  constructor(private appHttpService: AppHttpService) {
    const selectedElection = localStorage.getItem(SELECTED_ELECTION_TO_WORK_ON);

    this.appHttpService
      .getAppContext(selectedElection)
      .pipe(take(1))
      .subscribe((res) => {
        this.user.next(res.data.user);
        this.election.next(res.data.election);
        this.municipalityName.next(res.data.municipalityName);
        this.externalWebUrl.next(res.data.externalWebUrl);
      });
  }

  changeElectionToWorkOn(election: AppElection) {
    this.election.next(election);
  }
}

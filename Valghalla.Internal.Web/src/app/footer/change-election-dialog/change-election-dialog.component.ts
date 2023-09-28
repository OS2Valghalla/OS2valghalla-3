import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { Observable, combineLatest } from 'rxjs';
import { AppHttpService } from 'src/app/app-http.service';
import { GlobalStateService } from 'src/app/global-state.service';
import { AppElection } from 'src/app/models/app-election';
import { SELECTED_ELECTION_TO_WORK_ON } from 'src/shared/constants/election';
import { FormDialogEvent } from 'src/shared/models/ux/formDialog';
import { SubSink } from 'subsink';

@Component({
  selector: 'app-change-election-dialog',
  templateUrl: './change-election-dialog.component.html',
})
export class ChangeElectionDialogComponent implements OnInit, OnDestroy {
  private subs = new SubSink();

  loading = true;

  allElections: AppElection[];

  constructor(
    private formBuilder: FormBuilder,
    private globalStateService: GlobalStateService,
    private appHttpService: AppHttpService,
  ) {}

  readonly form = this.formBuilder.group({
    electionId: ['', Validators.required],
  });

  ngOnInit(): void {
    this.subs.sink = combineLatest({
      elections: this.appHttpService.getElectionsToWorkOn(),
      currentElection: this.globalStateService.election$,
    }).subscribe((d) => {
      this.loading = false;
      this.allElections = d.elections.data;
      this.form.setValue({
        electionId: d.currentElection?.id,
      });
    });
  }

  ngOnDestroy(): void {
    this.subs.unsubscribe();
  }

  submit(formEvent: FormDialogEvent) {
    localStorage.setItem(SELECTED_ELECTION_TO_WORK_ON, this.form.value.electionId);

    formEvent.pipe(
      new Observable((subscriber) => {
        const election = this.allElections.find(i => i.id == this.form.value.electionId);
        this.globalStateService.changeElectionToWorkOn(election);
        subscriber.next();

        return () => subscriber.complete();
      }),
      (observable, handler) => {
        this.subs.sink = observable.subscribe(() => {
          handler.next();
        });
      },
    );
  }
}

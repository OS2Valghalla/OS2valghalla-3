import { Component, OnDestroy, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { take } from 'rxjs';
import { AuthService } from 'src/app/auth/auth.service';
import { SubSink } from 'subsink';

@Component({
  selector: 'app-task-registration-landing',
  templateUrl: './task-registration-landing.component.html',
})
export class TaskRegistrationLandingComponent implements OnInit, OnDestroy {
  private readonly subs = new SubSink();

  currentStep: number = 0;

  constructor(private readonly router: Router, private readonly authService: AuthService) {}

  ngOnInit(): void {
    this.subs.sink = this.authService.authorized$.pipe(take(1)).subscribe((authorized) => {
      if (typeof authorized === 'undefined' || authorized == null) {
        return;
      }

      if (authorized) {
        this.router.navigate(['/']);
      }
    });
  }

  ngOnDestroy(): void {
    this.subs.unsubscribe();
  }

  next() {
    this.currentStep++;
    window.scrollTo(0, 0);
  }

  cancel() {
    this.authService.logout(false);
  }
}

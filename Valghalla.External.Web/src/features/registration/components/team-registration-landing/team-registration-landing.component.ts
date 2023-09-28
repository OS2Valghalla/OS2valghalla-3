import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { take } from 'rxjs';
import { AuthService } from 'src/app/auth/auth.service';
import { RoutingNodes } from 'src/shared/constants/routing-nodes';
import { SubSink } from 'subsink';

@Component({
  selector: 'app-team-registration-landing',
  templateUrl: './team-registration-landing.component.html',
})
export class TeamRegistrationLandingComponent implements OnInit {
  private readonly subs = new SubSink();

  currentStep: number = 0;
  flag: boolean;

  private hashValue: string;

  constructor(
    private readonly router: Router,
    private readonly route: ActivatedRoute,
    private readonly authService: AuthService,
  ) {}

  ngOnInit(): void {
    this.flag = this.route.snapshot.queryParamMap.get('flag') == 'true';
    this.hashValue = this.route.snapshot.paramMap.get(RoutingNodes.Id);

    if (this.flag) {
      this.currentStep = 2;
    } else {
      this.subs.sink = this.authService.authorized$.pipe(take(1)).subscribe((authorized) => {
        if (typeof authorized === 'undefined' || authorized == null) {
          return;
        }

        if (authorized) {
          this.router.navigate(['/']);
        }
      });
    }
  }

  ngOnDestroy(): void {
    this.subs.unsubscribe();
  }

  next() {
    if (this.currentStep == 1) {
      const redirectUrl = [
        window.location.origin,
        RoutingNodes.Registration,
        RoutingNodes.TeamRegistration,
        this.hashValue,
      ].join('/') + "?flag=true";

      window.location.href = redirectUrl;
    }
    else {
      this.currentStep++;
      window.scrollTo(0, 0);
    }
  }

  cancel() {
    this.authService.logout(false);
  }
}

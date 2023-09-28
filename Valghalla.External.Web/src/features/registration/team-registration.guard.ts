import { Injectable } from '@angular/core';
import {
  ActivatedRouteSnapshot,
  CanActivate,
  CanActivateChild,
  CanMatch,
  Route,
  Router,
  RouterStateSnapshot,
  UrlSegment,
  UrlTree,
} from '@angular/router';
import { Observable, catchError, take, throwError } from 'rxjs';
import { AuthService } from '../../app/auth/auth.service';
import { RoutingNodes } from 'src/shared/constants/routing-nodes';
import { RegistrationHttpService } from './services/registration-http.service';

@Injectable()
export class TeamRegistrationGuard implements CanActivate, CanActivateChild, CanMatch {
  constructor(
    private readonly router: Router,
    private readonly authService: AuthService,
    private readonly registrationHttpService: RegistrationHttpService,
  ) {}

  canMatch(
    route: Route,
    segments: UrlSegment[],
  ): boolean | UrlTree | Observable<boolean | UrlTree> | Promise<boolean | UrlTree> {
    return this.resolve(undefined, undefined);
  }

  canActivateChild(
    childRoute: ActivatedRouteSnapshot,
    state: RouterStateSnapshot,
  ): boolean | UrlTree | Observable<boolean | UrlTree> | Promise<boolean | UrlTree> {
    const hashValue = childRoute.paramMap.get(RoutingNodes.Id);
    const flag = childRoute.queryParamMap.get('flag') == 'true';
    return this.resolve(hashValue, flag);
  }

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot,
  ): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
    const hashValue = route.paramMap.get(RoutingNodes.Id);
    const flag = route.queryParamMap.get('flag') == 'true';
    return this.resolve(hashValue, flag);
  }

  private resolve = (hashValue: string, flag: boolean) => {
    const redirectUrl = [
      window.location.origin,
      RoutingNodes.Registration,
      RoutingNodes.TeamRegistration,
      hashValue,
    ].join('/');

    return new Promise<boolean>((resolve) => {
      if (!this.authService.isSessionExist()) {
        this.authService.login(redirectUrl);
        resolve(false);
      }

      this.authService.authorized$.pipe(take(1)).subscribe((authorized) => {
        if (typeof authorized === 'undefined' || authorized == null) {
          return;
        }

        if (authorized) {
          if (flag) {
            resolve(true);
            return;
          }

          this.registrationHttpService
            .getTeamAccessStatus(hashValue)
            .pipe(
              take(1),
              catchError((err) => {
                this.router.navigate(['/']);
                return throwError(() => err);
              }),
            )
            .subscribe((res) => {
              if (res.isSuccess && res.data.joined) {
                this.router.navigate(['/']);
                resolve(false);
                return;
              }

              this.registrationHttpService
                .joinTeam(hashValue)
                .pipe(take(1))
                .subscribe((res) => {
                  if (res.isSuccess) {
                    this.router.navigate(['/']);
                  }
                });

              resolve(false);
            });

          return;
        }

        resolve(true);
      });
    });
  };
}

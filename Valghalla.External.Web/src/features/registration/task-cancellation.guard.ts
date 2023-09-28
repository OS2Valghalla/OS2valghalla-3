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
import { TaskHttpService } from '../tasks/services/task-http.service';
import { RoutingNodes } from 'src/shared/constants/routing-nodes';

@Injectable()
export class TaskCancellationGuard implements CanActivate, CanActivateChild, CanMatch {
  constructor(
    private readonly router: Router,
    private readonly authService: AuthService,
    private readonly taskHttpService: TaskHttpService,
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
    const invitationCode = childRoute.queryParamMap.get(RoutingNodes.TaskInvitationCode);
    return this.resolve(hashValue, invitationCode);
  }

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot,
  ): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
    const hashValue = route.paramMap.get(RoutingNodes.Id);
    const invitationCode = route.queryParamMap.get(RoutingNodes.TaskInvitationCode);
    return this.resolve(hashValue, invitationCode);
  }

  private resolve = (hashValue: string, invitationCode?: string) => {
    if (!hashValue) {
      this.router.navigate(['/']);
    }

    let redirectUrl = [
      window.location.origin,
      RoutingNodes.TaskRegistration,
      RoutingNodes.Cancellation,      
      hashValue,
    ].join('/');

    if (invitationCode) {
      redirectUrl += '?' + RoutingNodes.TaskInvitationCode + '=' + invitationCode;
    }

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
          this.taskHttpService
            .rejectTask(hashValue, invitationCode)
            .pipe(
              take(1),
              catchError((err) => {
                this.router.navigate(['/']);
                return throwError(() => err);
              }),
            )
            .subscribe();
          return;
        }

        resolve(true);
      });
    });
  };
}

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
import { Observable, take } from 'rxjs';
import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root',
})
export class AuthGuard implements CanActivate, CanActivateChild, CanMatch {
  constructor(private readonly router: Router, private readonly authService: AuthService) {}

  canMatch(
    route: Route,
    segments: UrlSegment[],
  ): boolean | UrlTree | Observable<boolean | UrlTree> | Promise<boolean | UrlTree> {
    return this.resolve();
  }

  canActivateChild(
    childRoute: ActivatedRouteSnapshot,
    state: RouterStateSnapshot,
  ): boolean | UrlTree | Observable<boolean | UrlTree> | Promise<boolean | UrlTree> {
    return this.resolve();
  }

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot,
  ): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
    return this.resolve();
  }

  private resolve = () => {
    return new Promise<boolean>((resolve) => {
      if (!this.authService.isSessionExist()) {
        this.router.navigate(['/']);
        resolve(false);
      }

      this.authService.authorized$.pipe(take(1)).subscribe((authorized) => {
        if (typeof authorized === 'undefined' || authorized == null) {
          return;
        }

        if (!authorized) {
          this.router.navigate(['/']);
        }

        resolve(authorized);
      });
    });
  };
}

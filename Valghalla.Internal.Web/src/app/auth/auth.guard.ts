import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, Route, Router, RouterStateSnapshot, UrlSegment, UrlTree } from '@angular/router';
import { Observable, take } from 'rxjs';
import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root',
})
export class AuthGuard  {
  constructor(private readonly router: Router, private readonly authService: AuthService) {}

  canMatch(
    route: Route, // eslint-disable-line
    segments: UrlSegment[], // eslint-disable-line
  ): boolean | UrlTree | Observable<boolean | UrlTree> | Promise<boolean | UrlTree> {
    return this.resolve();
  }

  canActivateChild(
    childRoute: ActivatedRouteSnapshot, // eslint-disable-line
    state: RouterStateSnapshot, // eslint-disable-line
  ): boolean | UrlTree | Observable<boolean | UrlTree> | Promise<boolean | UrlTree> {
    return this.resolve();
  }

  canActivate(
    route: ActivatedRouteSnapshot, // eslint-disable-line
    state: RouterStateSnapshot, // eslint-disable-line
  ): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
    return this.resolve();
  }

  private resolve = () => {
    return new Promise<boolean>((resolve) => {
      this.authService.authorized$.pipe(take(1)).subscribe((authorized) => {
        if (typeof authorized === 'undefined' || authorized == null) {
          return;
        }

        if (!authorized) {
          this.router.navigate(['_/unauthorized']);
        }

        resolve(authorized);
      });
    });
  };
}

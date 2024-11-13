import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, Route, RouterStateSnapshot, UrlSegment, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';
import { AuthService } from 'src/app/auth/auth.service';

@Injectable({
  providedIn: 'root',
})
export class AnonymousGuard  {
  constructor(private readonly authService: AuthService) {}

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
    // TODO: check cookies to see user already login in
    return false;
  };
}

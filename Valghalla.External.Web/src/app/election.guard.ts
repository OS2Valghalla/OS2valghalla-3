import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, Route, Router, RouterStateSnapshot, UrlSegment, UrlTree } from '@angular/router';
import { Observable, take } from 'rxjs';
import { GlobalStateService } from './global-state.service';

@Injectable({
  providedIn: 'root',
})
export class ElectionGuard  {
  constructor(private readonly router: Router, private readonly globalStateService: GlobalStateService) {}

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
      this.globalStateService.electionActivated$.pipe(take(1)).subscribe((electionActivated) => {
        if (electionActivated) {
          resolve(true);
        } else {
          this.router.navigate(['/']);
          resolve(false);
        }
      });
    });
  };
}

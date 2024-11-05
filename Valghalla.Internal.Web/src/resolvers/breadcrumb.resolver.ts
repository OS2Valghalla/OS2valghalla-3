import { Injectable } from '@angular/core';
import { Resolve, ActivatedRouteSnapshot } from '@angular/router';
import { TranslocoService } from '@ngneat/transloco';

@Injectable({
  providedIn: 'root',  // This ensures the resolver is provided at the root level
})
export class BreadcrumbResolver implements Resolve<string> {
  constructor(private translocoService: TranslocoService) {}

  resolve(route: ActivatedRouteSnapshot): string {
    const breadcrumbKey = route.data['breadcrumb'];
    return this.translocoService.translate(breadcrumbKey);
  }
}

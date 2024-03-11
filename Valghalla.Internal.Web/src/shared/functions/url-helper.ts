import { isDevMode } from '@angular/core';
import { environment } from '../../environments/environment';

export function getBaseApiUrl() {
  return !isDevMode() ? window.location.origin + '/api/' : environment.baseAddress;
}

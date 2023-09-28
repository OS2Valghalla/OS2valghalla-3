import { isDevMode } from '@angular/core';

export function getBaseApiUrl() {
  return !isDevMode() ? window.location.origin + '/api/' : 'base url for api endpoint in dev';
}

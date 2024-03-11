import { isDevMode } from '@angular/core';

export function getBaseApiUrl() {
  return !isDevMode() ? window.location.origin + '/api/' : 'base address to dev api';
}

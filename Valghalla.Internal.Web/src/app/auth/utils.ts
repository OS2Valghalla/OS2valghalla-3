import { HttpErrorResponse } from '@angular/common/http';
import { getBaseApiUrl } from 'src/shared/functions/url-helper';

export const TOKEN_EXPIRED = '__TOKEN_EXPIRED__';
const REDIRECT_URL = 'valghalla.redirectUrl';

export function getPingUrl() {
  return getBaseApiUrl() + 'auth/ping';
}

export function getLoginUrl() {
  return getBaseApiUrl() + 'auth/login';
}

export function getLogoutUrl() {
  return getBaseApiUrl() + 'auth/logout';
}

export function isTokenExpiredError(err: HttpErrorResponse) {
  return err.status == 401 && err.error == TOKEN_EXPIRED;
}

export function redirectToLoginIfNeeded(err: HttpErrorResponse): boolean {
  if (isTokenExpiredError(err)) {
    storeRedirectUrlIfNeeded();
    window.location.href = getLoginUrl();
    return true;
  }

  return false;
}

export function redirectIfNeeded() {
  const urlParams = new URLSearchParams(window.location.search);
  const redirectRequired = urlParams.get('redirect');

  if (redirectRequired) {
    const redirectUrl = localStorage.getItem(REDIRECT_URL);

    if (redirectUrl) {
      window.location.href = redirectUrl;
    }
  }
}

function storeRedirectUrlIfNeeded() {
  if (isRedirectRequired()) return;
  localStorage.setItem(REDIRECT_URL, window.location.href);
}

function isRedirectRequired() {
  const urlParams = new URLSearchParams(window.location.search);
  return !!urlParams.get('redirect');
}

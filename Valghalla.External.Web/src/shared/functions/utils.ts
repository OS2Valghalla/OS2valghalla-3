export function isSafari() {
  const userAgentString = navigator.userAgent;
  const chromeAgent = userAgentString.indexOf('Chrome') > -1;
  let safariAgent = userAgentString.indexOf('Safari') > -1;

  if (chromeAgent && safariAgent) {
    safariAgent = false;
  }

  return safariAgent;
}
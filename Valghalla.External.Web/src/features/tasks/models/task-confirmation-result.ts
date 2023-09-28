export interface TaskConfirmationResult {
  succeed: boolean;
  cprInvalid: boolean;
  conflicted: boolean;
  failedRuleIds: string[];
}

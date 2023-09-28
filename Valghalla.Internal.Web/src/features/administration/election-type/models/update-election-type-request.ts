export interface UpdateElectionTypeRequest {
  id: string;
  title: string;
  validationRuleIds: Array<string>;
}
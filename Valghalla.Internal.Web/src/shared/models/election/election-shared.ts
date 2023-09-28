export interface ElectionShared {
  id: string;
  title: string;
  active: boolean;
  electionDate: Date;
  electionTypeId?: string;
  electionTypeName?: string;
}

export interface GetAllUsersResponse {
  items: Array<GetAllUsersItem>;
  total: number;
}

export interface GetAllUsersItem {
  id: string;
  roleId: string;
  name: string;
  activated: boolean;
}

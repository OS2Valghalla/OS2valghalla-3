import { Role } from '../constants/role';

export const compareValues = (a: number | string | Date, b: number | string | Date, isAsc: boolean) => {
  return (a < b ? -1 : 1) * (isAsc ? 1 : -1);
};

export const hasPermission = (currentRole: string, requiredRole: Role) => {
  switch (requiredRole) {
    case Role.administrator: {
      return currentRole == Role.administrator;
    }
    case Role.editor: {
      return currentRole == Role.administrator || currentRole == Role.editor;
    }
    case Role.reader: {
      return currentRole == Role.administrator || currentRole == Role.editor || currentRole == Role.reader;
    }
    default: {
      return false;
    }
  }
};

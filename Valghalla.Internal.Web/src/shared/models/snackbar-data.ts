
export const enum SnackTypes {
  Info = 0,
  Success = 1, 
  Error = 2, 
  Warning = 3,
}
  

export class SnackbarData {
  message: string;

  body?: string;

  htmlContent?: string;

  snackType: SnackTypes;

  showCloseButton?: boolean;
}
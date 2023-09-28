import { Observable } from 'rxjs';

export interface FormDialogEvent {
  /**
   * Attach observable request when submit with form dialog
   * @param observable Wrap observable request when submitting form to handle unexpected errors
   * @param executor Use observable request when submitting form with form handler
   * @returns
   */
  pipe: <T>(
    observable: Observable<T>,
    executor: (observable: Observable<T>, handler: FormDialogHandler) => void,
  ) => void;
}

export interface FormDialogHandler {
  /**
   * Close dialog and optionally display notifcation message
   * @param msg plain text or localization key
   */
  next: (msg?: string | { msg: string; formResponseData: any }) => void;

  /**
   * Show error message
   * @param msg plain text or localization key
   */
  error: (errors?: string[]) => void;
}

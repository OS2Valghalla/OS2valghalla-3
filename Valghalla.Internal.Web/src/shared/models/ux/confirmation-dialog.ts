export interface ConfirmationDialogData {
  title: string;
  subtitle?: string;
  content?: string;
  htmlContent?: string;
  additionalButton?: {
    label: string;
    onClick: () => void;
  };
}

export interface PageMenuItem {
  title: string;
  matIcon: string;
  onSelectItem?: () => void;
  children?: PageMenuItem[];
}
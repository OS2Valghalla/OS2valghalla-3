export interface AuditLogListingItem {
  id: string;
  pk2name?: string;
  pk2value?: string;
  col2name?: string;
  col2value?: string;
  col3name?: string;
  col3value?: string;
  eventTable: string;
  eventType: string;
  eventDescription?: string;
  doneBy?: string;
  eventDate: Date;
}

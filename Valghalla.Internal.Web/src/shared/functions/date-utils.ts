// Shared date utility helpers
// Add more helpers as needed (addDays, isBetween, startOfDay, etc.)
export class DateUtils {
  // Compare two dates by calendar day (year, month, date) ignoring time & timezone differences
  static sameDay(a: Date | string | null | undefined, b: Date | string | null | undefined): boolean {
    if (!a || !b) { return false; }
    const da = new Date(a);
    const db = new Date(b);
    return da.getFullYear() === db.getFullYear() && da.getMonth() === db.getMonth() && da.getDate() === db.getDate();
  }
}

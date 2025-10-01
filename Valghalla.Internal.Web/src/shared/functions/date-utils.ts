export class DateUtils {
  /**
   * Determines whether two date inputs fall on the same calendar day (year, month, and date).
   *
   * The comparison:
   * - Returns false if either input is null/undefined.
   * - Returns false if either input cannot be parsed into a valid Date.
   * - Otherwise compares UTC-independent calendar components (year, month, date) of the constructed Date objects.
   * @returns True if both represent the same calendar day; otherwise false.
   */
  static sameDay(a?: Date | string | null, b?: Date | string | null): boolean {
    if (!a || !b) return false;
    const da = new Date(a);
    const db = new Date(b);
    if (isNaN(da.getTime()) || isNaN(db.getTime())) return false;
    return da.getFullYear() === db.getFullYear()
      && da.getMonth() === db.getMonth()
      && da.getDate() === db.getDate();
  }
}

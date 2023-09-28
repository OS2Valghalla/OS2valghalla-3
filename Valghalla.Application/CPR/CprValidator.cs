namespace Valghalla.Application.CPR
{
    public static class CprValidator
    {
        public static bool IsCvrValid(string cvr)
        {
            // 8 chars exactly
            if (cvr == null || cvr.Length != 8)
            {
                return false;
            }

            // only numbers
            foreach (char c in cvr)
            {
                if (c < '0' || c > '9')
                {
                    return false;
                }
            }

            return true;
        }

        public static bool IsCprValid(string cpr)
        {
            // 10 chars exactly
            if (cpr == null || cpr.Length != 10)
            {
                return false;
            }

            // only numbers
            foreach (char c in cpr)
            {
                if (c < '0' || c > '9')
                {
                    return false;
                }
            }

            // first 2 should be 1-31
            int days = Int32.Parse(cpr.Substring(0, 2));
            if (days < 1 || days > 31)
            {
                return false;
            }

            // second 2 should be 1-12
            int months = Int32.Parse(cpr.Substring(2, 2));
            if (months < 1 || months > 12)
            {
                return false;
            }

            return true;
        }
    }
}

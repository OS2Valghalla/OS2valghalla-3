namespace Valghalla.Application.Exceptions
{
    public class CprException : Exception
    {
    }

    public class CprInputInvalidException : CprException
    {
        public string Cpr { get; init; }

        public CprInputInvalidException(string cpr) => Cpr = cpr;
    }

    public class CvrInputInvalidException : CprException
    {
        public string Cvr { get; init; }

        public CvrInputInvalidException(string cvr) => Cvr = cvr;
    }
}

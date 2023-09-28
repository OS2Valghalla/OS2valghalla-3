namespace Valghalla.Application.DigitalPost
{
    public class DigitalPostException : Exception
    {
        public string Details { get; init; }

        public DigitalPostException(string msg, string details) : base(msg)
        {
            Details = details;
        }
    }
}

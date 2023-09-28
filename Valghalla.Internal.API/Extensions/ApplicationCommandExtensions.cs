using Valghalla.Application.Abstractions.Messaging;

namespace Valghalla.Internal.Application.Abstractions.Messaging
{
    public static class ApplicationCommandExtensions
    {
        public const string QUERY_PARAM_CONFIRMED = "confirmed";

        public static ConfirmationCommand Apply(this ConfirmationCommand command, HttpContext httpContext)
        {
            if (httpContext.Request.Query.TryGetValue(QUERY_PARAM_CONFIRMED, out var value))
            {
                if (bool.TryParse(value, out var confimed))
                {
                    command.Confirmed = confimed;
                }
            }

            return command;
        }
    }
}

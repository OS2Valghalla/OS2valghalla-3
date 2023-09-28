namespace Valghalla.Application.Exceptions
{
    public class UserException : Exception
    {
        public UserException() { }

        public UserException(string message) : base(message) { }

        public UserException(Exception innerException) : base("Internal server error", innerException) { }
    }

    public class NotFoundIdentifierClaimException : UserException { }

    public class NotFoundSocialIdpUserIdClaimException : UserException { }

    public class UnableToCastToUserResponseException : UserException { }

    public class AnonymousUserException : UserException { }
}

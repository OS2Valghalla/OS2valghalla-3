namespace Valghalla.Worker.Exceptions
{
    public class ExternalException : Exception
    {
        public string Content { get; init; }

        public string ShortContent { get; init; }

        public string? Details { get; init; }

        public ExternalException(string content, string shortContent, Exception ex) : base("Error", ex) => (Content, ShortContent) = (content, shortContent);
        public ExternalException(string content, string shortContent, string details, Exception ex) : base("Error", ex) => (Content, ShortContent, Details) = (content, shortContent, details);
    }
}

namespace Valghalla.Application.Exceptions
{
    public class QueryEngineException : Exception
    {
        public QueryEngineException() { }

        public QueryEngineException(string message): base(message) { }
    }

    public class QueryEngineUnableToAnalyzeExpressionException : QueryEngineException { }

    public class QueryEngineUnhandledDynamicCallException : QueryEngineException { }
}

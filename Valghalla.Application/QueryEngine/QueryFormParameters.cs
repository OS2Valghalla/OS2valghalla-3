using Valghalla.Application.Abstractions.Messaging;

namespace Valghalla.Application.QueryEngine
{
    public abstract record QueryFormParameters : IQuery<Response<QueryFormInfo>>
    {
    }

    public sealed record VoidQueryFormParameters : QueryFormParameters
    {

    }
}

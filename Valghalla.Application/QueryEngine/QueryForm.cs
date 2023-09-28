using Valghalla.Application.Abstractions.Messaging;
using Valghalla.Application.QueryEngine.Values;

namespace Valghalla.Application.QueryEngine
{
    public abstract record QueryForm { }

    // QueryForm is based class for query data, one generic parameter is result mapping data and the other is query form parameters needed for getting filters
    // the base class contains paging properties and free text search
    public abstract record QueryForm<TQueryResultItem, TQueryFormParameters> : QueryForm, IQuery<Response<QueryResult<TQueryResultItem>>> where TQueryFormParameters : QueryFormParameters
    {
        public virtual Order? Order { get; init; } = null!;

        public FreeTextSearchValue? Search { get; init; } = null!;

        public virtual int Take { get; init; } = 50;

        public virtual int Skip { get; init; } = 0;
    }
}

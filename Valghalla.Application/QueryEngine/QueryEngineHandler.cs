using Valghalla.Application.Abstractions.Messaging;
using MediatR;

namespace Valghalla.Application.QueryEngine
{
    public class QueryEngineHandler<TQueryForm, TQueryResultItem, TQueryFormParameters> : IRequestHandler<TQueryForm, Response<QueryResult<TQueryResultItem>>>
        where TQueryForm : QueryForm<TQueryResultItem, TQueryFormParameters>
        where TQueryFormParameters : QueryFormParameters
    {
        private readonly IQueryEngineRepository<TQueryForm, TQueryResultItem, TQueryFormParameters> queryableRepository;

        public QueryEngineHandler(IQueryEngineRepository<TQueryForm, TQueryResultItem, TQueryFormParameters> queryableRepository)
        {
            this.queryableRepository = queryableRepository;
        }

        public async Task<Response<QueryResult<TQueryResultItem>>> Handle(TQueryForm form, CancellationToken cancellationToken)
        {
            if (form.Search != null)
            {
                form.Search.Value = form.Search.Value.ToLower();
            }

            var result = await queryableRepository.ExecuteQuery(form, cancellationToken);

            return Response.Ok(result);
        }
    }
}

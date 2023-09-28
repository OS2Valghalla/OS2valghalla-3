using Valghalla.Application.Abstractions.Messaging;
using MediatR;

namespace Valghalla.Application.QueryEngine
{
    public class QueryEngineFormHandler<TQueryForm, TQueryResultItem, TQueryFormParameters> : IRequestHandler<TQueryFormParameters, Response<QueryFormInfo>>
        where TQueryForm : QueryForm<TQueryResultItem, TQueryFormParameters>
        where TQueryFormParameters : QueryFormParameters
    {
        private readonly IQueryEngineRepository<TQueryForm, TQueryResultItem, TQueryFormParameters> queryableRepository;

        public QueryEngineFormHandler(IQueryEngineRepository<TQueryForm, TQueryResultItem, TQueryFormParameters> queryableRepository)
        {
            this.queryableRepository = queryableRepository;
        }

        public async Task<Response<QueryFormInfo>> Handle(TQueryFormParameters @params, CancellationToken cancellationToken)
        {
            var result = await queryableRepository.GetQueryFormInfo(@params, cancellationToken);

            return Response.Ok(result);
        }
    }
}

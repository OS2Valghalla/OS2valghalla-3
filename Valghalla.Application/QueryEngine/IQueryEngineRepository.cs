namespace Valghalla.Application.QueryEngine
{
    public interface IQueryEngineRepository<TQueryForm, TQueryResultItem, TQueryFormParameters>
        where TQueryForm : QueryForm<TQueryResultItem, TQueryFormParameters>
        where TQueryFormParameters : QueryFormParameters
    {
        abstract Task<QueryResult<TQueryResultItem>> ExecuteQuery(TQueryForm form, CancellationToken cancellationToken);

        Task<QueryFormInfo> GetQueryFormInfo(TQueryFormParameters @params, CancellationToken cancellationToken);
    }
}

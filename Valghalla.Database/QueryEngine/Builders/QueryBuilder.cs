using Valghalla.Application.QueryEngine;

namespace Valghalla.Database.QueryEngine.Builders
{
    public abstract class QueryBuilder<TQueryForm, TQueryResultItem, TQueryFormParameters, TEntity>
        where TEntity : class
        where TQueryForm : QueryForm<TQueryResultItem, TQueryFormParameters>
        where TQueryFormParameters : QueryFormParameters
    {
        protected readonly QueryEngineRepository<TQueryForm, TQueryResultItem, TQueryFormParameters, TEntity> repository;
        protected readonly IQueryable<TEntity> queryable;

        public QueryBuilder(QueryEngineRepository<TQueryForm, TQueryResultItem, TQueryFormParameters, TEntity> repository, IQueryable<TEntity> queryable)
        {
            this.repository = repository;
            this.queryable = queryable;
        }
    }
}

using Valghalla.Application.QueryEngine;
using Valghalla.Application.QueryEngine.Values;

namespace Valghalla.Database.QueryEngine.Builders
{
    public class FreeTextQueryBuilder<TQueryForm, TQueryResultItem, TQueryFormParameters, TEntity> : QueryBuilder<TQueryForm, TQueryResultItem, TQueryFormParameters, TEntity>
        where TEntity : class
        where TQueryForm : QueryForm<TQueryResultItem, TQueryFormParameters>
        where TQueryFormParameters: QueryFormParameters
    {
        public Func<IQueryable<TEntity>, FreeTextSearchValue, TQueryForm, IQueryable<TEntity>> QueryFunc { get; private set; } = null!;

        public FreeTextQueryBuilder(QueryEngineRepository<TQueryForm, TQueryResultItem, TQueryFormParameters, TEntity> repository, IQueryable<TEntity> queryable) : base(repository, queryable)
        {
        }

        public QueryEngineRepository<TQueryForm, TQueryResultItem, TQueryFormParameters, TEntity> Use(Func<IQueryable<TEntity>, FreeTextSearchValue, IQueryable<TEntity>> statement)
        {
            QueryFunc = (queryable, query, _) => statement(queryable, query);
            return repository;
        }

        public QueryEngineRepository<TQueryForm, TQueryResultItem, TQueryFormParameters, TEntity> Use(Func<IQueryable<TEntity>, FreeTextSearchValue, TQueryForm, IQueryable<TEntity>> statement)
        {
            QueryFunc = statement;
            return repository;
        }
    }
}

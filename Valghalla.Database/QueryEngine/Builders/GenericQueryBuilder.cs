using Valghalla.Application.QueryEngine;

namespace Valghalla.Database.QueryEngine.Builders
{
    public class GenericQueryBuilder<TQueryForm, TQueryResultItem, TQueryFormParameters, TEntity> : QueryBuilder<TQueryForm, TQueryResultItem, TQueryFormParameters, TEntity>
        where TEntity : class
        where TQueryForm : QueryForm<TQueryResultItem, TQueryFormParameters>
        where TQueryFormParameters : QueryFormParameters
    {
        public GenericQueryBuilder(QueryEngineRepository<TQueryForm, TQueryResultItem, TQueryFormParameters, TEntity> repository, IQueryable<TEntity> queryable) : base(repository, queryable)
        {
        }
    }

    public class GenericQueryBuilder<TQueryForm, TQueryResultItem, TQueryValue, TQueryFormParameters, TEntity> : GenericQueryBuilder<TQueryForm, TQueryResultItem, TQueryFormParameters, TEntity>
        where TEntity : class
        where TQueryForm : QueryForm<TQueryResultItem, TQueryFormParameters>
        where TQueryFormParameters : QueryFormParameters
    {
        public Func<IQueryable<TEntity>, TQueryValue, TQueryForm, IQueryable<TEntity>> QueryFunc { get; private set; } = null!;

        public GenericQueryBuilder(QueryEngineRepository<TQueryForm, TQueryResultItem, TQueryFormParameters, TEntity> repository, IQueryable<TEntity> queryable) : base(repository, queryable)
        {
        }

        public QueryEngineRepository<TQueryForm, TQueryResultItem, TQueryFormParameters, TEntity> Use(Func<IQueryable<TEntity>, TQueryValue, IQueryable<TEntity>> statement)
        {
            QueryFunc = (queryable, query, _) => statement(queryable, query);
            return repository;
        }

        public QueryEngineRepository<TQueryForm, TQueryResultItem, TQueryFormParameters, TEntity> Use(Func<IQueryable<TEntity>, TQueryValue, TQueryForm, IQueryable<TEntity>> statement)
        {
            QueryFunc = statement;
            return repository;
        }
    }
}

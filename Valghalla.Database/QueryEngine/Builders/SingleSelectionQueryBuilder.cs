using Valghalla.Application.QueryEngine;
using Valghalla.Application.QueryEngine.Values;

namespace Valghalla.Database.QueryEngine.Builders
{
    public class SingleSelectionQueryBuilder<TQueryForm, TQueryResultItem, TQueryFormParameters, TEntity> : QueryBuilder<TQueryForm, TQueryResultItem, TQueryFormParameters, TEntity>
        where TEntity : class
        where TQueryForm : QueryForm<TQueryResultItem, TQueryFormParameters>
        where TQueryFormParameters : QueryFormParameters
    {
        public SingleSelectionQueryBuilder(QueryEngineRepository<TQueryForm, TQueryResultItem, TQueryFormParameters, TEntity> repository, IQueryable<TEntity> queryable) : base(repository, queryable)
        {
        }
    }

    public class SingleSelectionQueryBuilder<TQueryForm, TQueryResultItem, TQueryFormParameters, TEntity, TValue> : SingleSelectionQueryBuilder<TQueryForm, TQueryResultItem, TQueryFormParameters, TEntity>
        where TEntity : class
        where TQueryForm : QueryForm<TQueryResultItem, TQueryFormParameters>
        where TQueryFormParameters: QueryFormParameters
    {
        public Func<IQueryable<TEntity>, SingleSelectionFilterValue<TValue>, TQueryForm, IQueryable<TEntity>> QueryFunc { get; private set; } = null!;
        public Func<TQueryFormParameters, Task<IEnumerable<SelectOption<TValue>>>> OptionsFunc { get; private set; } = null!;
        public bool NoQuery { get; private set; } = false;

        public SingleSelectionQueryBuilder(QueryEngineRepository<TQueryForm, TQueryResultItem, TQueryFormParameters, TEntity> repository, IQueryable<TEntity> queryable) : base(repository, queryable)
        {
        }

        public QueryEngineRepository<TQueryForm, TQueryResultItem, TQueryFormParameters, TEntity> Use(Func<IQueryable<TEntity>, SingleSelectionFilterValue<TValue>, IQueryable<TEntity>> statement)
        {
            QueryFunc = (queryable, query, _) => statement(queryable, query);
            return repository;
        }

        public QueryEngineRepository<TQueryForm, TQueryResultItem, TQueryFormParameters, TEntity> Use(Func<IQueryable<TEntity>, SingleSelectionFilterValue<TValue>, TQueryForm, IQueryable<TEntity>> statement)
        {
            QueryFunc = statement;
            return repository;
        }

        public SingleSelectionQueryBuilder<TQueryForm, TQueryResultItem, TQueryFormParameters, TEntity, TValue> With(Func<TQueryFormParameters, Task<IEnumerable<SelectOption<TValue>>>> statement)
        {
            OptionsFunc = statement;
            return this;
        }

        public QueryEngineRepository<TQueryForm, TQueryResultItem, TQueryFormParameters, TEntity> UseNoQuery()
        {
            NoQuery = true;
            return repository;
        }
    }
}

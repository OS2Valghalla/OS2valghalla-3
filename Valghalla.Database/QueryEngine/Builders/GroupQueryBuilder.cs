using Valghalla.Application.Exceptions;
using Valghalla.Application.QueryEngine;
using Valghalla.Application.QueryEngine.Values;
using System.Linq.Expressions;

namespace Valghalla.Database.QueryEngine.Builders
{
    public class GroupQueryBuilder<TQueryForm, TQueryResultItem, TQueryFormParameters, TEntity> : QueryBuilder<TQueryForm, TQueryResultItem, TQueryFormParameters, TEntity>
        where TEntity : class
        where TQueryForm : QueryForm<TQueryResultItem, TQueryFormParameters>
        where TQueryFormParameters : QueryFormParameters
    {
        public Func<IQueryable<TEntity>, TQueryForm, IQueryable<TEntity>> QueryFunc { get; private set; } = null!;

        public List<string> PropertyNames { get; private set; } = new List<string>();

        public GroupQueryBuilder(QueryEngineRepository<TQueryForm, TQueryResultItem, TQueryFormParameters, TEntity> repository, IQueryable<TEntity> queryable) : base(repository, queryable)
        {
        }

        public GroupQueryBuilder<TQueryForm, TQueryResultItem, TQueryFormParameters, TEntity> For(Expression<Func<TQueryForm, BooleanFilterValue?>> expression)
        {
            CachePropertyNames(expression.Body);
            return this;
        }

        public GroupQueryBuilder<TQueryForm, TQueryResultItem, TQueryFormParameters, TEntity> For(Expression<Func<TQueryForm, FreeTextSearchValue?>> expression)
        {
            CachePropertyNames(expression.Body);
            return this;
        }

        public GroupQueryBuilder<TQueryForm, TQueryResultItem, TQueryFormParameters, TEntity> For<TValue>(Expression<Func<TQueryForm, SingleSelectionFilterValue<TValue>?>> expression)
        {
            CachePropertyNames(expression.Body);
            return this;
        }

        public GroupQueryBuilder<TQueryForm, TQueryResultItem, TQueryFormParameters, TEntity> For<TValue>(Expression<Func<TQueryForm, MultipleSelectionFilterValue<TValue>?>> expression)
        {
            CachePropertyNames(expression.Body);
            return this;
        }

        public GroupQueryBuilder<TQueryForm, TQueryResultItem, TQueryFormParameters, TEntity> For<TQueryValue>(Expression<Func<TQueryForm, TQueryValue>> expression)
        {
            CachePropertyNames(expression.Body);
            return this;
        }

        public QueryEngineRepository<TQueryForm, TQueryResultItem, TQueryFormParameters, TEntity> Use(Func<IQueryable<TEntity>, TQueryForm, IQueryable<TEntity>> statement)
        {
            QueryFunc = statement;
            PropertyNames = PropertyNames.Distinct().ToList();
            repository.CacheGroupBuilder(this);
            return repository;
        }

        private void CachePropertyNames(Expression expression)
        {
            if (expression is MemberExpression memberExpression)
            {
                PropertyNames.Add(memberExpression.Member.Name);
                return;
            }

            throw new QueryEngineUnableToAnalyzeExpressionException();
        }
    }
}

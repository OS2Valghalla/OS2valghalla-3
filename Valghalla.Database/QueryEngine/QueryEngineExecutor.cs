using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Valghalla.Application.QueryEngine;

namespace Valghalla.Database.QueryEngine
{
    public sealed class QueryEngineExecutor<TQueryResultItem, TQueryFormParameters, TEntity>
        where TEntity: class
        where TQueryFormParameters : QueryFormParameters
    {
        public IQueryable<TEntity> Queryable { get; init; }

        private readonly QueryForm<TQueryResultItem, TQueryFormParameters> form;
        private readonly IMapper mapper;

        public QueryEngineExecutor(IQueryable<TEntity> queryable, QueryForm<TQueryResultItem, TQueryFormParameters> form, IMapper mapper)
        {
            Queryable = queryable;
            this.form = form;
            this.mapper = mapper;
        }
        public async Task<(IEnumerable<TKeyResult>, IEnumerable<TQueryResultItem>, IEnumerable<TEntity>)> ExecuteAsync<TKeyResult>(Expression<Func<TEntity, TKeyResult>> keySelector, CancellationToken cancellationToken)
        {
            var keys = await Queryable.Select(keySelector).ToArrayAsync(cancellationToken);
            var entities = !keys.Any() ? Array.Empty<TEntity>() : await Queryable.Skip(form.Skip).Take(form.Take).ToArrayAsync(cancellationToken);
            var items = entities.Select(mapper.Map<TQueryResultItem>).ToArray();

            return (keys, items, entities);
        }
    }
}

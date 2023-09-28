using Valghalla.Application.QueryEngine;
using System.Linq.Expressions;

namespace System.Linq
{
    public static class OrderExtensions
    {
        public static IOrderedQueryable<TSource> SortBy<TSource, TKey>(this IQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector, Order order)
        {
            return order.Descending ? source.OrderByDescending(keySelector) : source.OrderBy(keySelector);
        }
    }
}

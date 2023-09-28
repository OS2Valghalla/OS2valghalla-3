using System.Linq.Expressions;

namespace Valghalla.Database.Extensions
{
    public static class QuerySortExtension
    {
        public static IOrderedQueryable<TSource> OrderBy<TSource, TKey>(this IQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector, bool isAscending)
        {
            return (isAscending ? source.OrderBy(keySelector) : source.OrderByDescending(keySelector));
        }
    }
}

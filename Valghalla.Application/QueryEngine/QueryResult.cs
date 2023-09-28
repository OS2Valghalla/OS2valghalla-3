namespace Valghalla.Application.QueryEngine
{
    public sealed record QueryResult<T> : QueryResult<T, Guid>
    {
        public QueryResult(IEnumerable<Guid> keys, IEnumerable<T> items) : base(keys, items) { }
    }

    public record QueryResult<T, K>
    {
        public IEnumerable<K> Keys { get; init; }

        public IEnumerable<T> Items { get; init; } = Enumerable.Empty<T>();

        public QueryResult(IEnumerable<K> keys, IEnumerable<T> items) => (Keys, Items) = (keys, items);
    }
}

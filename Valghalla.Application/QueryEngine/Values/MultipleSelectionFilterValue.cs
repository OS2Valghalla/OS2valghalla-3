using System.Text.Json.Serialization;

namespace Valghalla.Application.QueryEngine.Values
{
    public sealed record MultipleSelectionFilterValue<T> : QueryValue
    {
        public IEnumerable<T> Values { get; init; }

        [JsonConstructor]
        public MultipleSelectionFilterValue(IEnumerable<T> values) => Values = values;
    }
}

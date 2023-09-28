using System.Text.Json.Serialization;

namespace Valghalla.Application.QueryEngine.Values
{
    public sealed record SingleSelectionFilterValue<T> : QueryValue
    {
        public T Value { get; init; }

        [JsonConstructor]
        public SingleSelectionFilterValue(T value) => Value = value;
    }
}

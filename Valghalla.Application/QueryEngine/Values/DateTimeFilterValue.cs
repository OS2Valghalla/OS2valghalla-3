using System.Text.Json.Serialization;

namespace Valghalla.Application.QueryEngine.Values
{
    public sealed record DateTimeFilterValue : QueryValue
    {
        public DateTime Value { get; init; }

        [JsonConstructor]
        public DateTimeFilterValue(DateTime value) => Value = value;
    }
}

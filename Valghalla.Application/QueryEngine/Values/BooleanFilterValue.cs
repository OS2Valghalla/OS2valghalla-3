using System.Text.Json.Serialization;

namespace Valghalla.Application.QueryEngine.Values
{
    public sealed record BooleanFilterValue : QueryValue
    {
        public bool Value { get; init; }

        [JsonConstructor]
        public BooleanFilterValue(bool value) => Value = value;
    }
}

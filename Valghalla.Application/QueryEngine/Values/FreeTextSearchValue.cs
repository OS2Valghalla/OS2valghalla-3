using System.Text.Json.Serialization;

namespace Valghalla.Application.QueryEngine.Values
{
    public sealed record FreeTextSearchValue : QueryValue
    {
        public string Value { get; set; }

        [JsonConstructor]
        public FreeTextSearchValue(string value) => Value = value;
    }
}

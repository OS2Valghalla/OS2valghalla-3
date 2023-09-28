using System.Text.Json.Serialization;

namespace Valghalla.Application.QueryEngine
{
    public sealed record Order
    {
        public string Name { get; init; }

        public bool Descending { get; init; }

        [JsonConstructor]
        public Order(string name, bool descending) => (Name, Descending) = (name, descending);
    }
}

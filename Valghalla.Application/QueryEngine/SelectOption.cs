namespace Valghalla.Application.QueryEngine
{
    public record SelectOption<T>
    {
        public T Value { get; init; }

        public string Label { get; init; }

        public SelectOption(T value, string label) => (Value, Label) = (value, label);
    }
}

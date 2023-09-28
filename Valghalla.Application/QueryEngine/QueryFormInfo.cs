namespace Valghalla.Application.QueryEngine
{
    public enum QueryFormPropertyType
    {
        Generic = 0,
        Boolean = 1,
        FreeText = 2,
        SingleSelection = 3,
        MutipleSelection = 4,
        DateTime = 5,
    }

    public record QueryFormInfo
    {
        public string Properties { get; init; }

        public string Filters { get; init; }

        public QueryFormInfo(string properties, string filters) => (Properties, Filters) = (properties, filters);
    }
}

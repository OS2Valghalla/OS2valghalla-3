namespace Valghalla.Internal.Application.Modules.Analyze.Requests
{
    public class SaveAnalyzeQueryRequest
    {
        public int? QueryId { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool IsGlobal { get; set; }
        public CreateQueryRequest? CreateNewQueryRequest { get; set; }
    }
}

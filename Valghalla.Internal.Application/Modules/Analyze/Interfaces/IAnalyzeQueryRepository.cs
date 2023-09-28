using Valghalla.Internal.Application.Modules.Analyze.Responses;

namespace Valghalla.Internal.Application.Modules.Analyze.Interfaces
{
    public interface IAnalyzeQueryRepository
    {
        Task<AnalyzeResult> GetQueryResult(Guid electionId, int queryId, int skip, int? take = 10);
        IList<AnalyzeListTypeResponse> GetAnalyzePrimaryListTypes();
        Task<AnalyzeListTypeSelectionsResponse> GetAnalyzeSelections(int listTypeId, CancellationToken cancellationToken);
        IList<AnalyzeQueryResponse> GetSavedQueriesByUserId(Guid userId);
        Task<AnalyzeQueryDetailResponse> GetSavedQueryDetail(int queryId, CancellationToken cancellationToken);
        Task<bool> IsNameUsed(string name, CancellationToken cancellationToken);
    }
}

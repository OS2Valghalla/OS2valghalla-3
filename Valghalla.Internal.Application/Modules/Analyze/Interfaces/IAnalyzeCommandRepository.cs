using Valghalla.Internal.Application.Modules.Analyze.Requests;

namespace Valghalla.Internal.Application.Modules.Analyze.Interfaces
{
    public interface IAnalyzeCommandRepository
    {
        Task<int> CreateQuery(CreateQueryRequest query, CancellationToken cancellationToken);
        Task<int> SaveQuery(SaveAnalyzeQueryRequest request, CancellationToken cancellationToken);
    }
}

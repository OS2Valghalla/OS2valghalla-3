using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using Valghalla.Application.User;
using Valghalla.Database;
using Valghalla.Database.Entities.Analyze;
using Valghalla.Internal.Application.Modules.Analyze.Interfaces;
using Valghalla.Internal.Application.Modules.Analyze.Requests;

namespace Valghalla.Internal.Infrastructure.Modules.Analyze
{
    internal class AnalyzeCommandRepository : IAnalyzeCommandRepository
    {
        private readonly DataContext context;
        private readonly UserContext userContext;

        public AnalyzeCommandRepository(DataContext dataContext, IUserContextProvider userContextProvider)
        {
            context = dataContext;
            userContext = userContextProvider.CurrentUser;
        }

        /// <summary>
        /// Create analyze query
        /// </summary>
        /// <returns>Query result</returns>
        public async Task<int> CreateQuery(CreateQueryRequest request, CancellationToken cancellationToken)
        {
            var query = new QueryEntity
            {
                Name = string.Empty,
                SystemName = string.Empty,
                IsTemplate = false,
                IsGlobal = false,
                CreatedBy = userContext.UserId,
                ListTypeId = request.ListTypeId
            };

            var allColumnIds = request.ColumnIds.Concat(request.RelatedListTypes.SelectMany(x => x.ColumnIds));
            var columns = await context.Analyze_Columns.AsNoTracking().Where(x => allColumnIds.Contains(x.ColumnId)).ToListAsync(cancellationToken);

            context.Analyze_Queries.Add(query);
            await context.SaveChangesAsync(cancellationToken);
            var resultColumns = columns.Select(x =>
            {
                return new ResultColumnEntity
                {
                    ColumnId = x.ColumnId,
                    Ordinal = x.Ordinal,
                    QueryId = query.QueryId
                };
            });

            context.Analyze_ResultColumns.AddRange(resultColumns);
            await context.SaveChangesAsync(cancellationToken);

            return query.QueryId;
        }

        /// <summary>
        /// Create analyze query
        /// </summary>
        /// <returns>Query ID</returns>
        public async Task<int> SaveQuery(SaveAnalyzeQueryRequest request, CancellationToken cancellationToken)
        {
            int queryId = -1;
            if (request.QueryId.HasValue)
            {
                queryId = await SaveExistingQuery(request, cancellationToken);
            }
            else if (request.CreateNewQueryRequest != null)
            {
                queryId = await SaveNewQuery(request, cancellationToken);
            }

            return queryId;
        }

        /// <summary>
        /// Save to existing query
        /// </summary>
        /// <returns>Query ID</returns>
        private async Task<int> SaveExistingQuery(SaveAnalyzeQueryRequest request, CancellationToken cancellationToken)
        {
            var query = await context.Analyze_Queries.FirstOrDefaultAsync(x => x.QueryId == request.QueryId, cancellationToken);

            if (query == null) return -1;

            query.Name = request.Name;
            query.SystemName = request.Name;
            query.IsGlobal = request.IsGlobal;

            context.Analyze_Queries.Update(query);
            await context.SaveChangesAsync(cancellationToken);

            return query.QueryId;
        }

        /// <summary>
        /// Save whole new query
        /// </summary>
        /// <returns>Query ID</returns>
        private async Task<int> SaveNewQuery(SaveAnalyzeQueryRequest request, CancellationToken cancellationToken)
        {
            var query = new QueryEntity
            {
                Name = request.Name,
                SystemName = request.Name,
                IsTemplate = false,
                IsGlobal = request.IsGlobal,
                CreatedBy = userContext.UserId,
                ListTypeId = request.CreateNewQueryRequest.ListTypeId
            };

            var allColumnIds = request.CreateNewQueryRequest.ColumnIds.Concat(request.CreateNewQueryRequest.RelatedListTypes.SelectMany(x => x.ColumnIds));
            var columns = await context.Analyze_Columns.AsNoTracking().Where(x => allColumnIds.Contains(x.ColumnId)).ToListAsync(cancellationToken);

            context.Analyze_Queries.Add(query);
            await context.SaveChangesAsync(cancellationToken);
            var resultColumns = columns.Select(x =>
            {
                return new ResultColumnEntity
                {
                    ColumnId = x.ColumnId,
                    Ordinal = x.Ordinal,
                    QueryId = query.QueryId
                };
            });

            context.Analyze_ResultColumns.AddRange(resultColumns);
            await context.SaveChangesAsync(cancellationToken);

            return query.QueryId;
        }
    }
}

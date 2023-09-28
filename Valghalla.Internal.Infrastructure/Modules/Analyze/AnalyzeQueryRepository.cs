using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using Valghalla.Database;
using Valghalla.Database.Entities.Analyze;
using Valghalla.Internal.Application.Modules.Analyze.Interfaces;
using Valghalla.Internal.Application.Modules.Analyze.Responses;

namespace Valghalla.Internal.Infrastructure.Modules.Analyze
{
    internal class AnalyzeQueryRepository : IAnalyzeQueryRepository
    {
        private readonly DataContext context;
        private readonly IMapper mapper;

        public AnalyzeQueryRepository(DataContext dataContext, IMapper mapper)
        {
            context = dataContext;
            this.mapper = mapper;
        }

        public async Task<bool> IsNameUsed(string name, CancellationToken cancellationToken)
        {
            return await context.Analyze_Queries.AnyAsync(q => q.Name == name, cancellationToken);
        }

        /// <summary>
        /// Get's result from an existing analyze query. 
        /// </summary>
        /// <param name="electionId"></param>
        /// <param name="queryId"></param>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <returns>JSON string with header columns, data and total amount. The header columns do include the name of the property, the shown name, the sorting of the headers and the datatype.</returns>
        public async Task<AnalyzeResult> GetQueryResult(Guid electionId, int queryId, int skip, int? take = 10)
        {
            AnalyzeResult result = null;

            QueryEntity? query = await context.Analyze_Queries.AsNoTracking()
                .Include(x => x.ResultColumns)
                .ThenInclude(x => x.Column)
                .Include(x => x.Filters)
                .ThenInclude(x => x.FilterColumns).ThenInclude(x => x.ColumnOperator).ThenInclude(x => x.Operator)
                .Include(x => x.Filters)
                .ThenInclude(x => x.FilterColumns).ThenInclude(x => x.FilterColumnValues)
                .Include(x => x.SortColumns)
                .ThenInclude(x => x.Column)
                .FirstOrDefaultAsync(x => x.QueryId.Equals(queryId));

            if (query == null)
                return result;

            var view = await context.Analyze_ListTypes.AsNoTracking().FirstOrDefaultAsync(x => x.ListTypeId.Equals(query.ListTypeId));

            if (view == null)
                return result;


            switch (view.View)
            {
                /*case ViewsConstants.BuildingView:
                    result = await GetResultSet(context.BuildingView.AsNoTracking().Where(x => x.ElectionId.Equals(electionId)), query, skip, take);
                    break;
                case ViewsConstants.RoomView:
                    result = await GetResultSet(context.RoomView.AsNoTracking().Where(x => x.ElectionId.Equals(electionId)), query, skip, take);
                    break;
                case ViewsConstants.ElectoralDistrictView:
                    result = await GetResultSet(context.ElectoralDistrictView.AsNoTracking().Where(x => x.ElectionId.Equals(electionId)), query, skip, take);
                    break;
                case ViewsConstants.PersonView:
                    result = await GetResultSet(context.PersonView.AsNoTracking().Where(x => x.ElectionId.Equals(electionId)), query, skip, take);
                    break;
                case ViewsConstants.CourseOccassionView:
                    result = await GetResultSet(context.CourseOccassionView.AsNoTracking().Where(x => x.ElectionId.Equals(electionId)), query, skip, take);
                    break;
                case ViewsConstants.GroupView:
                    result = await GetResultSet(context.GroupView.AsNoTracking().Where(x => x.ElectionId.Equals(electionId)), query, skip, take);
                    break;
                case ViewsConstants.ApplicationView:
                    result = await GetResultSet(context.ApplicationView.AsNoTracking().Where(x => x.ElectionId.Equals(electionId)), query, skip, take);
                    break;
                case ViewsConstants.AssignmentView:
                    result = await GetResultSet(context.AssignmentView.AsNoTracking().Where(x => x.ElectionId.Equals(electionId)), query, skip, take);
                    break;*/
                default:
                    break;
            }

            return result;
        }

        private async Task<AnalyzeResult> GetResultSet<T>(IQueryable<T> values, QueryEntity query, int skip, int? take)
        {
            var columnNames = query.ResultColumns
                .OrderBy(x => x.Ordinal)
                .Select(x => x.Column.ColumnName);

            var headerColumns = query.ResultColumns
                .OrderBy(x => x.Ordinal)
                .Select(x => new AnalyzeResultColumn
                {
                    PropertyName = x.Column.ColumnName,
                    HeaderName = x.Column.Name,
                    Ordinal = x.Ordinal,
                    Type = x.Column.Datatype
                });

            var propertiesToIncludeString = "new { " + string.Join(",", columnNames) + " }";

            if (string.IsNullOrEmpty(propertiesToIncludeString))
                throw new ArgumentNullException(nameof(propertiesToIncludeString));

            var result = values.Select(propertiesToIncludeString);

            var propertiesToFilterString = "";

            foreach (var filter in query.Filters)
            {
                if (propertiesToFilterString != "")
                    propertiesToFilterString += " && ";

                foreach (var filterColumns in filter.FilterColumns)
                {
                    var datatypeId = filterColumns.Column.DatatypeId;
                    propertiesToFilterString += filterColumns.Column.ColumnName;
                    var filterOperator = filterColumns.ColumnOperator.Operator.Symbol;

                    foreach (var filterColumn in filterColumns.FilterColumnValues)
                    {
                        filterColumn.Value = filterColumn.Value.Replace("\"", "");

                        if (datatypeId == (int)Datatype.Numeric)
                            propertiesToFilterString += " " + filterOperator + filterColumn.Value;
                        else if (datatypeId == (int)Datatype.Date)
                            propertiesToFilterString += " " + filterOperator + DateTime.Parse(filterColumn.Value);
                        else
                            propertiesToFilterString += " " + filterOperator + " \"" + filterColumn.Value + "\"";
                    }
                }
            }

            if (!string.IsNullOrEmpty(propertiesToFilterString))
                result = result.Where(propertiesToFilterString);

            var sortingOrder = query.SortColumns
                .OrderBy(x => x.Ordinal)
                .Select(x => x.Column.ColumnName);

            var propertiesToSortString = string.Join(",", sortingOrder);

            if (!string.IsNullOrEmpty(propertiesToSortString))
                result = result.OrderBy(propertiesToSortString);

            result = result.Distinct();

            var list = take.HasValue ? await result.Skip(skip).Take(take.Value).ToDynamicListAsync()
                                    : await result.Skip(skip).ToDynamicListAsync();
            var count = result.Count();

            var returnObject = new AnalyzeResult
            {
                HeaderMappings = headerColumns,
                Data = list,
                Count = count
            };

            return returnObject;
        }

        enum Datatype
        {
            Numeric = 1,
            String = 2,
            Date = 3,
        }

        /// <summary>
        /// Get all analyze tables
        /// </summary>
        /// <returns>Analyze list types</returns>
        public IList<AnalyzeListTypeResponse> GetAnalyzePrimaryListTypes()
        {
            var listTypes = context.Analyze_ListTypes.AsNoTracking()
                .Where(x => x.IsPrimary == true)
                .Select(mapper.Map<AnalyzeListTypeResponse>)
                .ToList();
            return listTypes;
        }

        /// <summary>
        /// Get selections based on list type id
        /// </summary>
        /// <returns>Analyze selections</returns>
        public async Task<AnalyzeListTypeSelectionsResponse> GetAnalyzeSelections(int listTypeId, CancellationToken cancellationToken)
        {
            var selection = await context.Analyze_ListTypes.AsNoTracking()
                .Include(x => x.ListTypeColumns)
                .ThenInclude(x => x.Column)
                .Include(x => x.RelatedListTypes)
                .ThenInclude(x => x.ListTypeColumns)
                .ThenInclude(x => x.Column)
                .FirstOrDefaultAsync(x => x.ListTypeId == listTypeId, cancellationToken);

            if (selection == null) return null;

            var result = mapper.Map<AnalyzeListTypeSelectionsResponse>(selection);
            var related = result.ListTypeColumns.Where(x => x.ListTypeId == result.ListTypeId && x.Column.ListTypeId != result.ListTypeId);
            result.ListTypeColumns = result.ListTypeColumns.Where(x => x.ListTypeId == result.ListTypeId && x.Column.ListTypeId == result.ListTypeId).ToList();

            foreach (var relatedListTypes in result.RelatedListTypes)
            {
                relatedListTypes.ListTypeColumns = related.Where(x => x.Column.ListTypeId == relatedListTypes.ListTypeId).ToList();
            }

            return result;
        }

        /// <summary>
        /// Get all saved queries
        /// </summary>
        /// <returns>Analyze Queries</returns>
        public IList<AnalyzeQueryResponse> GetSavedQueriesByUserId(Guid userId)
        {
            var queries = context.Analyze_Queries.AsNoTracking()
                .Where(x => !string.IsNullOrEmpty(x.Name) && !string.IsNullOrEmpty(x.SystemName) && (x.CreatedBy.Equals(userId) || x.IsGlobal))
                .Select(mapper.Map<AnalyzeQueryResponse>)
                .ToList();
            return queries;
        }

        /// <summary>
        /// Get saved query detail
        /// </summary>
        /// <returns>Analyze list types</returns>
        public async Task<AnalyzeQueryDetailResponse> GetSavedQueryDetail(int queryId, CancellationToken cancellationToken)
        {
            var query = await context.Analyze_Queries.AsNoTracking()
                .Include(x => x.ResultColumns)
                .FirstOrDefaultAsync(x => x.QueryId == queryId, cancellationToken);

            if (query == null) { return null; }

            var result = mapper.Map<AnalyzeQueryDetailResponse>(query);
            return result;
        }
    }
}

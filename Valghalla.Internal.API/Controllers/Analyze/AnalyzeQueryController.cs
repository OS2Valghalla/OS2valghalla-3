using MediatR;
using Microsoft.AspNetCore.Mvc;
using Valghalla.Application.User;
using Valghalla.Integration.Auth;
using Valghalla.Internal.Application.Modules.Analyze.Queries;

namespace Valghalla.Internal.API.Controllers.Analyze
{
    /// <summary>
    /// Endpoint for getting the analyze
    /// </summary>
    [ApiController]
    [Route("api/analyze")]
    [UserAuthorize(RoleEnum.Administrator)]
    public class AnalyzeQueryController : ControllerBase
    {
        private readonly ISender sender;
        private readonly UserContext userContext;

        public AnalyzeQueryController(ISender sender, IUserContextProvider userContextProvider)
        {
            this.sender = sender;
            this.userContext = userContextProvider.CurrentUser;
        }

        /// <summary>
        /// Get analyze list types
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns>list types</returns>
        [HttpGet("getprimarylisttypes")]
        public async Task<IActionResult> GetPrimaryListTypes(CancellationToken cancellationToken)
        {
            var query = new GetAnalyzePrimaryListTypesQuery();
            var result = await sender.Send(query, cancellationToken);

            return Ok(result);
        }

        /// <summary>
        /// Get analyze selections by on list type id
        /// </summary>
        /// <param name="listTypeId"></param>
        /// <param name="cancellationToken"></param>
        [HttpGet("getanalyzeselections")]
        public async Task<IActionResult> GetAnalyzeSelections(int listTypeId, CancellationToken cancellationToken)
        {
            var query = new GetAnalyzeSelectionsQuery(listTypeId);
            var result = await sender.Send(query, cancellationToken);

            return Ok(result);
        }

        /// <summary>
        /// get query result
        /// </summary>
        /// <param name="electionId"></param>
        /// <param name="queryId"></param>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <param name="cancellationToken"></param>
        [HttpGet("getqueryresult")]
        public async Task<IActionResult> GetQueryResult(Guid electionId, int queryId, int skip, int take, CancellationToken cancellationToken)
        {
            var query = new GetQueryResultQuery(electionId, queryId, skip, take);
            var result = await sender.Send(query, cancellationToken);

            return Ok(result);
        }

        /// <summary>
        /// get saved query
        /// </summary>
        /// <param name="cancellationToken"></param>
        [HttpGet("getsavedqueries")]
        public async Task<IActionResult> GetSavedQueries(CancellationToken cancellationToken)
        {
            var query = new GetSavedAnalyzeQueriesQuery(userContext.UserId);
            var result = await sender.Send(query, cancellationToken);

            return Ok(result);
        }

        /// <summary>
        /// get saved query detail
        /// </summary>
        /// <param name="queryId"></param>
        /// <param name="cancellationToken"></param>
        [HttpGet("getsavedquerydetail")]
        public async Task<IActionResult> GetSavedQueryDetail(int queryId, CancellationToken cancellationToken)
        {
            var query = new GetSavedAnalyzeQueryDetailQuery(queryId);
            var result = await sender.Send(query, cancellationToken);

            return Ok(result);
        }

        /// <summary>
        /// export query to excel
        /// </summary>
        /// <param name="electionId"></param>
        /// <param name="queryId"></param>
        /// <param name="cancellationToken"></param>
        [HttpGet("exportquerytoexcel")]
        public async Task<IActionResult> ExportQueryToExcel(Guid electionId, int queryId, CancellationToken cancellationToken)
        {
            var query = new ExportQueryToExcelQuery(electionId, queryId);
            var result = await sender.Send(query, cancellationToken);

            var ms = result.Data;

            if (ms == null)
                return BadRequest();
            
            ms.Position = 0;
            ms.Close();
            var byteArray = ms.ToArray();

            HttpContext.Response.Headers.Add("content-disposition", "attachment; filename=AnalyzeExport_" + DateTime.Now.ToString("yyyy-MM-dd") + ".xls");
            HttpContext.Response.ContentType = "application/vnd.ms-excel";
            HttpContext.Response.ContentLength = byteArray.Length;

            return File(byteArray, "application/vnd.ms-excel");
        }
    }
}

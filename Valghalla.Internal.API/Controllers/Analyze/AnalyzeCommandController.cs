using MediatR;
using Microsoft.AspNetCore.Mvc;
using Valghalla.Integration.Auth;
using Valghalla.Internal.Application.Modules.Analyze.Commands;
using Valghalla.Internal.Application.Modules.Analyze.Requests;

namespace Valghalla.Internal.API.Controllers.Analyze
{
    /// <summary>
    /// Endpoint for getting the analyze
    /// </summary>
    [ApiController]
    [Route("api/analyze")]
    [UserAuthorize(RoleEnum.Administrator)]
    public class AnalyzeCommandController : ControllerBase
    {
        private readonly ISender sender;

        public AnalyzeCommandController(ISender sender)
        {
            this.sender = sender;
        }


        /// <summary>
        /// create an analyze query
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        [HttpPost("createanalyzyequery")]
        public async Task<IActionResult> CreateAnalyzeQuery([FromBody] CreateQueryRequest request, CancellationToken cancellationToken)
        {
            var query = new CreateAnalyzeQueryCommand(request);
            var queryId = await sender.Send(query, cancellationToken);

            return Ok(queryId);
        }

        /// <summary>
        /// save analyze query
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        [HttpPost("saveanalyzyequery")]
        public async Task<IActionResult> SaveAnalyzeQuery([FromBody] SaveAnalyzeQueryRequest request, CancellationToken cancellationToken)
        {
            var query = new SaveAnalyzeQueryCommand(request);
            var queryId = await sender.Send(query, cancellationToken);

            return Ok(queryId);
        }
    }
}

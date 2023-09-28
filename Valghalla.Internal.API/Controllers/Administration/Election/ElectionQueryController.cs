using MediatR;
using Microsoft.AspNetCore.Mvc;
using Valghalla.Application.QueryEngine;
using Valghalla.Integration.Auth;
using Valghalla.Internal.Application.Modules.Administration.Election.Queries;

namespace Valghalla.Internal.API.Controllers.Administration.Election
{
    [ApiController]
    [Route("api/administration/election")]
    [UserAuthorize(RoleEnum.Administrator)]
    public class ElectionQueryController : ControllerBase
    {
        private readonly ISender sender;

        public ElectionQueryController(ISender sender)
        {
            this.sender = sender;
        }

        [HttpGet("getelection")]
        public async Task<IActionResult> GetElectionAsync(Guid id, CancellationToken cancellationToken)
        {
            var query = new GetElectionQuery(id);
            var result = await sender.Send(query, cancellationToken);
            return Ok(result);
        }

        [HttpGet("getelectioncommunicationconfigurations")]
        public async Task<IActionResult> GetElectionCommunicationConfigurationsAsync(Guid id, CancellationToken cancellationToken)
        {
            var query = new GetElectionCommunicationConfigurationsQuery(id);
            var result = await sender.Send(query, cancellationToken);
            return Ok(result);
        }

        [HttpPost("queryelectionlisting")]
        public async Task<IActionResult> QueryElectionListingAsync(ElectionListingQueryForm form, CancellationToken cancellationToken)
        {
            var response = await sender.Send(form, cancellationToken);
            return Ok(response);
        }

        [HttpPost("getelectionlistingqueryform")]
        public async Task<IActionResult> GetElectionListingQueryFormAsync(VoidQueryFormParameters form, CancellationToken cancellationToken)
        {
            var response = await sender.Send(form, cancellationToken);
            return Ok(response);
        }
    }
}

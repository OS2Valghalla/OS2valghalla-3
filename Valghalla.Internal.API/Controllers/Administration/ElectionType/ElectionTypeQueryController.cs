using MediatR;
using Microsoft.AspNetCore.Mvc;
using Valghalla.Application.QueryEngine;
using Valghalla.Integration.Auth;
using Valghalla.Internal.Application.Modules.Administration.ElectionType.Queries;

namespace Valghalla.Internal.API.Controllers.Administration.ElectionType
{
    [ApiController]
    [Route("api/administration/electiontype")]
    [UserAuthorize(RoleEnum.Administrator)]
    public class ElectionTypeQueryController : ControllerBase
    {
        private readonly ISender sender;

        public ElectionTypeQueryController(ISender sender)
        {
            this.sender = sender;
        }

        [HttpGet("getelectiontype")]
        public async Task<IActionResult> GetElectionTypeAsync(Guid id, CancellationToken cancellationToken)
        {
            var query = new GetElectionTypeQuery(id);
            var result = await sender.Send(query, cancellationToken);
            return Ok(result);
        }

        [HttpPost("queryelectiontypelisting")]
        public async Task<IActionResult> QueryElectionTypeListingAsync(ElectionTypeListingQueryForm form, CancellationToken cancellationToken)
        {
            var response = await sender.Send(form, cancellationToken);
            return Ok(response);
        }

        [HttpPost("getelectiontypelistingqueryform")]
        public async Task<IActionResult> GetElectionTypeListingQueryFormAsync(VoidQueryFormParameters form, CancellationToken cancellationToken)
        {
            var response = await sender.Send(form, cancellationToken);
            return Ok(response);
        }
    }
}

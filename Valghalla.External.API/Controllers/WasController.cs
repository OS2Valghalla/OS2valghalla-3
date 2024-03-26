using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Valghalla.Application.Saml;
using Valghalla.Application.User;

namespace Valghalla.External.API.Controllers
{
    [ApiController]
    public class WasController : ControllerBase
    {
        private readonly ISender sender;
        private readonly ISaml2AuthService saml2AuthService;
        private readonly IUserContextProvider userContextProvider;

        public WasController(
            ISender sender,
            ISaml2AuthService saml2AuthService,
            IUserContextProvider userContextProvider)
        {
            this.sender = sender;
            this.saml2AuthService = saml2AuthService;
            this.userContextProvider = userContextProvider;
        }

        [HttpGet]
        [Route("was")]
        [AllowAnonymous]
        public IActionResult Redirect()
        {
            return Redirect("https://www.was.digst.dk/os2valghalla-dk");
        }
    }
}

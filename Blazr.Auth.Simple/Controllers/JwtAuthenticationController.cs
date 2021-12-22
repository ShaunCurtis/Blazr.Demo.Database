/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================

using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Blazr.Auth.WASM.Web.Controllers
{
    [ApiController]
    public class JwtAuthenticationController : ControllerBase
    {
        private IJwtAuthenticationIssuer _authenticationIssuer;

        public JwtAuthenticationController(IJwtAuthenticationIssuer authenticationIssuer)
            =>  _authenticationIssuer = authenticationIssuer;

        [Route("/api/authenticate/login-jwttoken")]
        [HttpPost]
        public async Task<IActionResult> GetLoginTokenAsync(IdentityLoginCredentials credentials)
            => this.Ok(await _authenticationIssuer.GetAuthenticationTokenAsync(credentials));

        [Route("/api/authenticate/refresh-jwttoken")]
        [HttpPost]
        public IActionResult RefreshLoginToken(SessionToken currentSessionToken)
            => this.Ok(_authenticationIssuer.RefreshAuthenticationToken(currentSessionToken));

        [Route("/api/authenticate/validate-jwttoken")]
        [HttpPost]
        public IActionResult ValidateToken(SessionToken currentSessionToken)
            => this.Ok(_authenticationIssuer.TryValidateToken(currentSessionToken));
    }
}

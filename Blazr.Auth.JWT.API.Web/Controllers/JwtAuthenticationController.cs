/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================


namespace Blazr.Auth.JWT.API.Web;

[ApiController]
public class JwtAuthenticationController : ControllerBase
{
    private IJwtAuthenticationIssuer _authenticationIssuer;

    public JwtAuthenticationController(IJwtAuthenticationIssuer authenticationIssuer)
        => _authenticationIssuer = authenticationIssuer;

    [AllowAnonymous]
    [Route("/api/authenticate/login-jwttoken")]
    [HttpPost]
    public async Task<IActionResult> GetLoginTokenAsync(IdentityLoginCredentials credentials)
        => this.Ok(await _authenticationIssuer.GetAuthenticationTokenAsync(credentials));

    [AllowAnonymous]
    [Route("/api/authenticate/validate-jwttoken")]
    [HttpPost]
    public IActionResult ValidateToken(SessionToken currentSessionToken)
        => this.Ok(_authenticationIssuer.TryValidateToken(currentSessionToken));
}


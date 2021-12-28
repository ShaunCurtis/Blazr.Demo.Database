/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================


namespace Blazr.Auth.JWT.Simple.Core;

public class SimpleJwtServerClientAuthenticationService : BaseJwtClientAuthenticationService, IClientAuthenticationService
{
    private IJwtAuthenticationIssuer _authenticationIssuer;

    public SimpleJwtServerClientAuthenticationService(IJwtAuthenticationIssuer authenticationIssuer)
        => _authenticationIssuer = authenticationIssuer;

    protected override Task<SessionToken> GetTokenAsync(IdentityLoginCredentials credentials)
        => _authenticationIssuer.GetAuthenticationTokenAsync(credentials);

    protected override Task<bool> ValidateTokenAsync(SessionToken sessionToken)
        => Task.FromResult(_authenticationIssuer.TryValidateToken(sessionToken));
}


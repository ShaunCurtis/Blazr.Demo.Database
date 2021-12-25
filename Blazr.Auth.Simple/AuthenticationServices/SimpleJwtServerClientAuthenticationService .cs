/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================


namespace Blazr.Auth;

/// <summary>
/// JWT Service that Interfaces directly with the DI registered IJwtAuthenticationIssuer instance
/// </summary>
public class SimpleJwtServerClientAuthenticationService : BaseJwtClientAuthenticationService, IClientAuthenticationService
{
    private IJwtAuthenticationIssuer _authenticationIssuer;

    public SimpleJwtServerClientAuthenticationService(IJwtAuthenticationIssuer authenticationIssuer)
        => _authenticationIssuer = authenticationIssuer;

    protected override async Task<SessionToken> GetTokenAsync(IdentityLoginCredentials credentials)
    {
        var sessionToken = await _authenticationIssuer.GetAuthenticationTokenAsync(credentials);
        SessionTokenManagement.TryGetFromJwt(sessionToken.JwtToken, SimpleIdentityStore.AuthenticationType, out ClaimsPrincipal identity);
        this.NotifyAuthenticationChanged(identity);
        return sessionToken;
    }

    protected override Task<bool> ValidateTokenAsync(SessionToken sessionToken)
        => Task.FromResult(_authenticationIssuer.TryValidateToken(sessionToken));
}


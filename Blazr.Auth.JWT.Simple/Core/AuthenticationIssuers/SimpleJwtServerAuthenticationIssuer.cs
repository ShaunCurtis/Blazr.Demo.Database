/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================

using Microsoft.Extensions.Options;

namespace Blazr.Auth.JWT.Simple.Core;

public class SimpleJwtServerAuthenticationIssuer : IJwtAuthenticationIssuer
{
    private readonly JwtTokenSetup _jwtTokenSetup;
    private readonly IIdentityDataBroker _identityDataBroker;

    public SimpleJwtServerAuthenticationIssuer(IOptions<JwtTokenSetup> jwtTokenSetup, IIdentityDataBroker identityDataBroker)
    {
        _identityDataBroker = identityDataBroker;
        _jwtTokenSetup = jwtTokenSetup!.Value;
    }

    public async Task<SessionToken> GetAuthenticationTokenAsync(IdentityLoginCredentials userCredentials)
    {
        var identity = await _identityDataBroker.GetIdentityAsync(userCredentials);

        return identity is not null && identity.Identity is not null
            ? SessionTokenManagement.GetNewSessionToken(identity!.Claims.ToArray(), _jwtTokenSetup)
            : new SessionToken();
    }

    public bool TryValidateToken(SessionToken thisSessionToken, out ClaimsPrincipal claimsPrincipal)
        => SessionTokenManagement.TryValidateToken(thisSessionToken, _jwtTokenSetup, out claimsPrincipal);

    public bool TryValidateToken(SessionToken thisSessionToken)
        => SessionTokenManagement.TryValidateToken(thisSessionToken, _jwtTokenSetup, out ClaimsPrincipal claimsPrincipal);
}

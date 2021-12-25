/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================


namespace Blazr.Auth.Core;

public interface IJwtAuthenticationIssuer
{
    /// <summary>
    /// Gets new SessionToken for the Identity defined userCredentials
    /// </summary>
    /// <param name="userCredentials">Credentials used to validate the identity</param>
    /// <returns>Either valid SessionToken if authentication is successful or an empty SessionToken</returns>
    public Task<SessionToken> GetAuthenticationTokenAsync(IdentityLoginCredentials userCredentials);

    /// <summary>
    /// Tries to Validate the current token
    /// </summary>
    /// <param name="thisSessionToken">current SessionToken</param>
    /// <param name="claimsPrincipal">The validated ClaimsPrincipal</param>
    /// <returns>True if validated</returns>
    public bool TryValidateToken(SessionToken thisSessionToken, out ClaimsPrincipal claimsPrincipal);

    /// <summary>
    /// Tries to Validate the current token
    /// </summary>
    /// <param name="thisSessionToken">current SessionToken</param>
    /// <returns>True if validated</returns>
    public bool TryValidateToken(SessionToken thisSessionToken);
}


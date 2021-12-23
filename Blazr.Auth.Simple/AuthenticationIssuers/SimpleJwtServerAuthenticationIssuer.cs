/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================

using Microsoft.Extensions.Options;
using System.Diagnostics;

namespace Blazr.Auth
{
    public class SimpleJwtServerAuthenticationIssuer : IJwtAuthenticationIssuer
    {
        private readonly JwtTokenSetup _jwtTokenSetup;
        private Dictionary<Guid, SessionToken> _currentSessions = new Dictionary<Guid, SessionToken>();
        private string _debugLabel = "==> Simple JWT Issuer";


        public SimpleJwtServerAuthenticationIssuer(IOptions<JwtTokenSetup> jwtTokenSetup)
        {
            _jwtTokenSetup = jwtTokenSetup.Value;
        }

        /// <summary>
        /// Gets new SessionToken for the Identity defined userCredentials
        /// </summary>
        /// <param name="userCredentials">Credentials used to validate the identity</param>
        /// <returns>Either valid SessionToken if authentication is successful or an empty SessionToken</returns>
        public Task<SessionToken> GetAuthenticationTokenAsync(IdentityLoginCredentials userCredentials)
        {

            var isAuthenticated = SimpleIdentities.TryGetIdentity(userCredentials, out ClaimsPrincipal identity);

            var sessionToken = SessionTokenManagement.GetNewSessionToken(identity.Claims.ToArray(), _jwtTokenSetup);

            Debug.WriteLine(
                isAuthenticated
                ? $"{_debugLabel} - Issued Token - {sessionToken.SessionId} - for {userCredentials.UserName}"
                : $"{_debugLabel} - Identity {userCredentials.UserName} unknown."
                );

            return Task.FromResult(sessionToken);
        }

        /// <summary>
        /// Tries to Validate the current token
        /// </summary>
        /// <param name="thisSessionToken">current SessionToken</param>
        /// <param name="claimsPrincipal">The validated ClaimsPrincipal</param>
        /// <returns>True if validated</returns>
        public bool TryValidateToken(SessionToken thisSessionToken, out ClaimsPrincipal claimsPrincipal)
        {
            var isValid = SessionTokenManagement.TryValidateToken(thisSessionToken, _jwtTokenSetup, out claimsPrincipal);

            Debug.WriteLine(isValid
                ? $"{_debugLabel} - Validated Token - {thisSessionToken.SessionId} - for {claimsPrincipal.Identity!.Name}"
                : $"{_debugLabel} - Rejected Validation for Token - {thisSessionToken.SessionId} - for {claimsPrincipal.Identity!.Name}"
                );

            return isValid;
        }

        /// <summary>
        /// Tries to Validate the current token
        /// </summary>
        /// <param name="thisSessionToken">current SessionToken</param>
        /// <returns>True if validated</returns>
        public bool TryValidateToken(SessionToken thisSessionToken)
        {
            var isValid = SessionTokenManagement.TryValidateToken(thisSessionToken, _jwtTokenSetup, out ClaimsPrincipal claimsPrincipal);

            Debug.WriteLine(isValid
                ? $"{_debugLabel} - Validated Token - {thisSessionToken.SessionId} - for {claimsPrincipal.Identity!.Name}"
                : $"{_debugLabel} - Rejected Validation for Token - {thisSessionToken.SessionId} - for {claimsPrincipal.Identity!.Name}"
                );

            return isValid;
        }
    }
}

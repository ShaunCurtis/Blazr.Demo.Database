/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================


namespace Blazr.Auth
{
    public class SimpleJwtServerClientAuthenticationService : BaseJwtClientAuthenticationService, IClientAuthenticationService
    {
        private IJwtAuthenticationIssuer _authenticationIssuer;

        public SimpleJwtServerClientAuthenticationService(ILocalStorageService localStorageService, IJwtAuthenticationIssuer authenticationIssuer)
            : base(localStorageService: localStorageService)
            => _authenticationIssuer = authenticationIssuer;

        protected override Task<SessionToken> GetTokenAsync(IdentityLoginCredentials credentials)
            => _authenticationIssuer.GetAuthenticationTokenAsync(credentials);

        protected override Task<SessionToken> ValidateTokenAsync(SessionToken sessionToken)
        {
            //TODO - need to check this is correct and returning a token for Validation.  Why not a bool?
            var isValid = _authenticationIssuer.TryValidateToken(sessionToken, out ClaimsPrincipal principal);
            return isValid
                ? Task.FromResult(sessionToken)
                : Task.FromResult(new SessionToken());
        }
    }
}

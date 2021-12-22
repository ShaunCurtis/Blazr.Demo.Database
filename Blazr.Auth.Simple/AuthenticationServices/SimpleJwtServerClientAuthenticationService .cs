/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================


namespace Blazr.Auth
{
    public class SimpleJwtServerClientAuthenticationService : BaseJwtClientAuthenticationService, IClientAuthenticationService
    {
        private IJwtAuthenticationIssuer _authenticationProvider;

        public SimpleJwtServerClientAuthenticationService(ILocalStorageService localStorageService, IJwtAuthenticationIssuer authenticationProvider)
            : base(localStorageService: localStorageService)
            => _authenticationProvider = authenticationProvider;

        protected override Task<SessionToken> GetTokenAsync(IdentityLoginCredentials credentials)
            => _authenticationProvider.GetAuthenticationTokenAsync(credentials);

        protected override Task<SessionToken> ValidateTokenAsync(SessionToken sessionToken)
        {
            //TODO - need to check this is correct and returning a token for Validation.  Why not a bool?
            var isValid = _authenticationProvider.TryValidateToken(sessionToken, out ClaimsPrincipal principal);
            return isValid
                ? Task.FromResult(sessionToken)
                : Task.FromResult(new SessionToken());
        }
    }
}

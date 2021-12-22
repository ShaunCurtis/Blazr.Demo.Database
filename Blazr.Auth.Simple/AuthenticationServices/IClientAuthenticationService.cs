/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================


namespace Blazr.Auth
{
    /// <summary>
    ///  Interface defining the client side Service for authentication
    /// </summary>
    public interface IClientAuthenticationService
    {
        public Task<bool> LogInAsync(IdentityLoginCredentials credentials);

        public Task<bool> LogOutAsync();

        public Task<SessionToken> GetCurrentSessionTokenAsync();

        public event EventHandler<AuthenticationChangedEventArgs> AuthenticationChanged;

    }
}

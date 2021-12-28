/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================

namespace Blazr.Auth.JWT.Simple.Core;

public abstract class BaseJwtClientAuthenticationService : IClientAuthenticationService
{
    protected SessionToken _sessionToken = new SessionToken();

    public event EventHandler<AuthenticationChangedEventArgs>? AuthenticationChanged;

    public async Task<bool> LogInAsync(IdentityLoginCredentials credentials)
    {
        _sessionToken = await this.GetTokenAsync(credentials);
        var isvalidated = SessionTokenManagement.TryGetFromSessionToken(_sessionToken, "local", out ClaimsPrincipal principal);
        this.AuthenticationChanged?.Invoke(this, AuthenticationChangedEventArgs.NewAuthenticationChangedEventArgs(principal));
        return isvalidated;
    }

    public Task<bool> LogOutAsync()
    {
        _sessionToken = new SessionToken();
        this.AuthenticationChanged?.Invoke(this, new AuthenticationChangedEventArgs());
        return Task.FromResult(true);
    }

    public SessionToken GetCurrentSessionToken()
        => _sessionToken;

    public ClaimsPrincipal GetCurrentIdentity()
        =>  SessionTokenManagement.TryGetFromSessionToken(_sessionToken, AppConstants.AuthenticationType, out ClaimsPrincipal principal)
            ? principal
            : new ClaimsPrincipal();

    protected abstract Task<SessionToken> GetTokenAsync(IdentityLoginCredentials credentials);

    protected abstract Task<bool> ValidateTokenAsync(SessionToken sessionToken);
}


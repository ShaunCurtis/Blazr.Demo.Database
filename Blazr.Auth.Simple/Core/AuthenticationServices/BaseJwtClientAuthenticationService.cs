/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================

namespace Blazr.Auth.Core;

public abstract class BaseJwtClientAuthenticationService : IClientAuthenticationService
{
    protected SessionToken _sessionToken = new SessionToken();

    public event EventHandler<AuthenticationChangedEventArgs>? AuthenticationChanged;

    public async Task<bool> LogInAsync(IdentityLoginCredentials credentials)
    {
        var sessionToken = await this.GetTokenAsync(credentials);
        _sessionToken = sessionToken;
        return true;
    }

    public Task<bool> LogOutAsync()
    {
        _sessionToken = new SessionToken();
        return Task.FromResult(true);
    }

    public Task<SessionToken> GetCurrentSessionTokenAsync()
        => Task.FromResult(_sessionToken);

    protected abstract Task<SessionToken> GetTokenAsync(IdentityLoginCredentials credentials);

    protected abstract Task<bool> ValidateTokenAsync(SessionToken sessionToken);
}


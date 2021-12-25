/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================


namespace Blazr.Auth.Core;

/// <summary>
/// Authentication State Provider that uses JWT tokens 
/// </summary>
public class SimpleJwtAuthenticationStateProvider : AuthenticationStateProvider, IDisposable
{
    private readonly IClientAuthenticationService _clientAuthenticationService;

    public SimpleJwtAuthenticationStateProvider(IClientAuthenticationService clientAuthenticationService)
    {
        _clientAuthenticationService = clientAuthenticationService;
        _clientAuthenticationService.AuthenticationChanged += this.OnAuthenticationChanged;
    }

    /// <summary>
    /// Override base class GetAuthenticationStateAsync
    /// </summary>
    /// <returns></returns>
    public async override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var sessionToken = await _clientAuthenticationService.GetCurrentSessionTokenAsync();

        return SessionTokenManagement.TryGetFromSessionToken(sessionToken, AppConstants.AuthenticationType, out ClaimsPrincipal claim)
            ? new AuthenticationState(claim)
            : new AuthenticationState(SessionTokenManagement.AnonymousClaimsPrincipal);
    }

    private void OnAuthenticationChanged(object? sender, AuthenticationChangedEventArgs e)
    {
        if (e.Identity is not null)
            this.NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(e.Identity)));
    }

    public void Dispose()
        => _clientAuthenticationService.AuthenticationChanged -= this.OnAuthenticationChanged;

}

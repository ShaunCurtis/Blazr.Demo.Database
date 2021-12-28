/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================

namespace Blazr.Auth.JWT.Simple.Core;

/// <summary>
/// Authentication State Provider that uses JWT tokens 
/// </summary>
public class ServiceAuthenticationStateProvider : AuthenticationStateProvider, IDisposable
{
    private readonly IClientAuthenticationService _clientAuthenticationService;

    public ServiceAuthenticationStateProvider(IClientAuthenticationService clientAuthenticationService)
    {
        _clientAuthenticationService = clientAuthenticationService;
        _clientAuthenticationService.AuthenticationChanged += this.OnAuthenticationChanged;
    }

    /// <summary>
    /// Override base class GetAuthenticationStateAsync
    /// </summary>
    /// <returns></returns>
    public override Task<AuthenticationState> GetAuthenticationStateAsync()
        => Task.FromResult(new AuthenticationState(_clientAuthenticationService.GetCurrentIdentity()));

    private void OnAuthenticationChanged(object? sender, AuthenticationChangedEventArgs e)
    {
        if (e.Identity is not null)
            this.NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(e.Identity)));
    }

    public void Dispose()
        => _clientAuthenticationService.AuthenticationChanged -= this.OnAuthenticationChanged;

}

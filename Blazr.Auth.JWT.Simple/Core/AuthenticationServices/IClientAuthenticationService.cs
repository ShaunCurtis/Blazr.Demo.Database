/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================


namespace Blazr.Auth.JWT.Simple.Core;

/// <summary>
///  Interface defining the client side Service for authentication
/// </summary>
public interface IClientAuthenticationService
{
    public Task<bool> LogInAsync(IdentityLoginCredentials credentials);

    public Task<bool> LogOutAsync();

    public SessionToken GetCurrentSessionToken();

    public ClaimsPrincipal GetCurrentIdentity();

    public event EventHandler<AuthenticationChangedEventArgs> AuthenticationChanged;
}


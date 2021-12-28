# Simple JWT Services

First the interface:

```csharp
public interface IClientAuthenticationService
{
    public Task<bool> LogInAsync(IdentityLoginCredentials credentials);
    public Task<bool> LogOutAsync();
    public SessionToken GetCurrentSessionToken();
    public ClaimsPrincipal GetCurrentIdentity();
    public event EventHandler<AuthenticationChangedEventArgs> AuthenticationChanged;
}
```

And a base abstract class.

`LogInAsync` is the core method. It:
1. Gets the token from the issuer based on the supplied credentials.  The implementation of `GetTokenAsync` depends on the child implementation.
2. Checks if the toek in valid
3. Invokes `AuthenticationChanged`.

```csharp
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
```

There are two implementations.

#### SimpleJwtServerClientAuthenticationService

`SimpleJwtServerClientAuthenticationService` interfaces directly with the instance of `IJwtAuthenticationIssuer` running in the DI container.  This is used in Server mode.

```csharp
public class SimpleJwtServerClientAuthenticationService : BaseJwtClientAuthenticationService, IClientAuthenticationService
{
    private IJwtAuthenticationIssuer _authenticationIssuer;

    public SimpleJwtServerClientAuthenticationService(IJwtAuthenticationIssuer authenticationIssuer)
        => _authenticationIssuer = authenticationIssuer;

    protected override Task<SessionToken> GetTokenAsync(IdentityLoginCredentials credentials)
        => _authenticationIssuer.GetAuthenticationTokenAsync(credentials);

    protected override Task<bool> ValidateTokenAsync(SessionToken sessionToken)
        => Task.FromResult(_authenticationIssuer.TryValidateToken(sessionToken));
}
```

#### SimpleJwtClientAuthenticationService

`SimpleJwtClientAuthenticationService` is the WASM implementation that interfaces with the API.

```csharp
public class SimpleJwtClientAuthenticationService : BaseJwtClientAuthenticationService, IClientAuthenticationService
{
    protected HttpClient _httpClient;

    public SimpleJwtClientAuthenticationService(HttpClient httpClient)
        => _httpClient = httpClient;

    protected override async Task<SessionToken> GetTokenAsync(IdentityLoginCredentials credentials)
    {
        SessionToken? newSessionToken = null;
        var response = await _httpClient.PostAsJsonAsync<IdentityLoginCredentials>(AppConstants.LogInUrl, credentials);
        if (response.IsSuccessStatusCode)
            newSessionToken = await response.Content.ReadFromJsonAsync<SessionToken>();
        return newSessionToken ?? new SessionToken();
    }

    protected override async Task<bool> ValidateTokenAsync(SessionToken sessionToken)
    {
        bool isvalidated = false;
        var response = await _httpClient.PostAsJsonAsync<SessionToken>(AppConstants.ValidateUrl, sessionToken);
        if (response.IsSuccessStatusCode)
            isvalidated = await response.Content.ReadFromJsonAsync<bool>();

        return isvalidated;
    }
}
```
/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================


namespace Blazr.Auth;

/// <summary>
/// JWT Service that uses the API Controller for Authentication
/// </summary>
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

        if (newSessionToken != null)
        {
            SessionTokenManagement.TryGetFromJwt(newSessionToken.JwtToken, SimpleIdentityStore.AuthenticationType, out ClaimsPrincipal identity);
            this.NotifyAuthenticationChanged(identity);
        }
        this.LogProcess(credentials, newSessionToken);
        return newSessionToken ?? new SessionToken();
    }

    protected override async Task<bool> ValidateTokenAsync(SessionToken sessionToken)
    {
        bool isvalidated = false;
        var response = await _httpClient.PostAsJsonAsync<SessionToken>(AppConstants.ValidateUrl, sessionToken);
        if (response.IsSuccessStatusCode)
            isvalidated = await response.Content.ReadFromJsonAsync<bool>();

        this.LogProcess(sessionToken, isvalidated);
        return isvalidated;
    }

    private void LogProcess(SessionToken sessionToken, bool isValidated)
    {
        var label = "==> Client";
        if (isValidated)
            Console.WriteLine($"{label} - Validated - {sessionToken.SessionId}.");

        else
            Console.WriteLine($"{label} - Validation failed  - {sessionToken.SessionId}.");
    }

    private void LogProcess(IdentityLoginCredentials credentials, SessionToken? newSessionToken)
    {
        var label = "==> Client";
        if (newSessionToken is null)
            Console.WriteLine($"{label} - Authenticate {credentials.UserName} - No Session Token returned.");
        else if (newSessionToken.IsEmpty)
            Console.WriteLine($"{label} - Authenticate {credentials.UserName} - Empty Token returned.");
        else
            Console.WriteLine($"{label} - Authenticate {credentials.UserName} - Validate with {newSessionToken.SessionId}.");
    }
}


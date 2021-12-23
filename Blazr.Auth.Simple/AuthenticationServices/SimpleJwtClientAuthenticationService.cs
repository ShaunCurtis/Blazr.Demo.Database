/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================


namespace Blazr.Auth;

public class SimpleJwtClientAuthenticationService : BaseJwtClientAuthenticationService, IClientAuthenticationService
{
    protected HttpClient _httpClient;

    public SimpleJwtClientAuthenticationService(ILocalStorageService localStorageService, HttpClient httpClient)
        : base(localStorageService: localStorageService)
        => _httpClient = httpClient;

    protected override async Task<SessionToken> GetTokenAsync(IdentityLoginCredentials credentials)
    {
        SessionToken? newSessionToken = null;
        var response = await _httpClient.PostAsJsonAsync<IdentityLoginCredentials>(AppConstants.LogInUrl, credentials);
        if (response.IsSuccessStatusCode)
            newSessionToken = await response.Content.ReadFromJsonAsync<SessionToken>();
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

    private void LogProcess(SessionToken sessionToken, SessionToken? newSessionToken)
    {
        var label = "==> Client";
        if (newSessionToken is null)
            Console.WriteLine($"{label} - Not Validated - No Session Token returned.");

        else if (newSessionToken.IsEmpty)
            Console.WriteLine($"{label} - Not Validated Empty Session Token returned.");

        else if (sessionToken is not null)
            Console.WriteLine(
                newSessionToken == sessionToken
                ? $"{label} - Validated Session Token - {sessionToken.SessionId}."
                : $"{label} - Validated Session Token - {sessionToken.SessionId} - Expired - New Token Issued - {newSessionToken.SessionId}."
                );
        else
            Console.WriteLine($"{label} - Attempt made to log null Session Tokens.");
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


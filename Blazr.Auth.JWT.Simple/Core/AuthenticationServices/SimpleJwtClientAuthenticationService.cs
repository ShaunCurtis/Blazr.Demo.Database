/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================


namespace Blazr.Auth.JWT.Simple.Core;

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


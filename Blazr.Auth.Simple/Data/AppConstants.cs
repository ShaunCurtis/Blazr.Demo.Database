/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================

namespace Blazr.Auth;

public static class AppConstants
{
    public const string LogInUrl = "/api/authenticate/login-jwttoken";

    public const string RefreshUrl = "/api/authenticate/refresh-jwttoken";

    public const string ValidateUrl = "/api/authenticate/validate-jwttoken";

    public const string JwtTokenLabel = "JwtToken";

    public static string AuthenticationType => SimpleIdentityStore.AuthenticationType;
}


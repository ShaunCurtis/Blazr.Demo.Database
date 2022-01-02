/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================

using Blazr.Auth.JWT.Simple.Core;

public static class SessionTokenManagement
{
    /// <summary>
    /// Method to issue a new SessionToken
    /// </summary>
    /// <param name="userClaims"></param>
    /// <param name="jwtTokenSetup"></param>
    /// <returns></returns>
    /// <exception cref="ApplicationException"></exception>
    public static SessionToken GetNewSessionToken(Claim[] userClaims, JwtTokenSetup jwtTokenSetup)
    {
        var jwtToken = GetNewJwtToken(userClaims, jwtTokenSetup);
        return new SessionToken() { JwtToken = jwtToken, IsAuthenticated = true };
    }

    /// <summary>
    /// Method to Validate a Session Token
    /// </summary>
    /// <param name="sessionToken">Issued Session Token</param>
    /// <param name="jwtTokenSetup">Token setup</param>
    /// <param name="claimsPrincipal">Claims principal from the Session Token</param>
    /// <returns></returns>
    public static bool TryValidateToken(SessionToken sessionToken, JwtTokenSetup jwtTokenSetup, out ClaimsPrincipal claimsPrincipal)
    {
        claimsPrincipal = new ClaimsPrincipal();
        bool isValid;
        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenValidationParameters = GetValidationParameters(jwtTokenSetup);
        try
        {
            claimsPrincipal = tokenHandler.ValidateToken(sessionToken.JwtToken, tokenValidationParameters, out SecurityToken validatedToken);
            isValid = validatedToken is not null;
        }
        catch (SecurityTokenException)
        {
            isValid = false;
        }
        catch (Exception)
        {
            throw;
        }
        return isValid;
    }

    /// <summary>
    /// Expiration Validation Method for a Seciurity Token
    /// Matches the Delegate pattern required for TokenValidationParameters.LiftimeValidator
    /// </summary>
    /// <param name="notBefore"></param>
    /// <param name="expires"></param>
    /// <param name="token"></param>
    /// <param name="params"></param>
    /// <returns></returns>
    public static bool JwtTokenLifetimeValidator(DateTime? notBefore, DateTime? expires, SecurityToken token, TokenValidationParameters @params)
        => (expires != null)
            ? expires > DateTime.Now
            : false;


    /// <summary>
    /// Method to Retrieve the Claims from a JwtToken
    /// </summary>
    /// <param name="jwt"></param>
    /// <returns></returns>
    public static IEnumerable<Claim> ParseClaimsFromJwt(string jwtToken)
    {
        var claims = new List<Claim>();
        if (!string.IsNullOrEmpty(jwtToken))
        {
            var payload = jwtToken.Split('.')[1];
            if (!string.IsNullOrEmpty(payload))
            {
                try
                {
                    var jsonBytes = ParseBase64(payload);
                    var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);
                    if (claims is not null && keyValuePairs is not null)
                        claims
                            .AddRange(keyValuePairs
                                .Select(kvp => new Claim(kvp.Key, kvp.Value.ToString() ?? String.Empty)
                                )
                            );
                }
                catch { }
            }
        }
        return claims ?? new List<Claim>();
    }

    /// <summary>
    /// Tries to get a ClaimsPrincipal from a SessionToken
    /// </summary>
    /// <param name="sessionToken">Session Token to use</param>
    /// <param name="authenticationType">Authentication Type to use in ClaimsPrincipal constructor</param>
    /// <param name="id">returned ClaimsPrincipal </param>
    /// <returns>True if successful</returns>
    public static bool TryGetFromSessionToken(SessionToken sessionToken, string authenticationType, out ClaimsPrincipal claimsPrincipal)
    {
        var isIdentity = sessionToken is not null && !sessionToken.IsEmpty;
        IEnumerable<Claim> claims = new List<Claim>();
        if (isIdentity)
        {
            claims = SessionTokenManagement.ParseClaimsFromJwt(sessionToken!.JwtToken);
            isIdentity = claims.Any();
        }
        claimsPrincipal = isIdentity
            ? new ClaimsPrincipal(new ClaimsIdentity(claims, authenticationType))
            : new ClaimsPrincipal();

        return isIdentity;
    }

    /// <summary>
    /// Gets the Guid Id for the supplied identity
    /// </summary>
    /// <param name="principal"></param>
    /// <returns></returns>
    public static Guid GetIdentityId(ClaimsPrincipal? principal)
    {
        if (principal is not null)
        {
            var claim = principal.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Sid);
            if (claim is not null && Guid.TryParse(claim.Value, out Guid id))
                return id;
        }
        return Guid.Empty;
    }

    public static ClaimsPrincipal AnonymousClaimsPrincipal
        => new ClaimsPrincipal(new ClaimsIdentity(null, ""));

    private static TokenValidationParameters GetValidationParameters(JwtTokenSetup jwtTokenSetup)
    {
        return new TokenValidationParameters
        {
            ValidateAudience = false,
            ValidateIssuer = true,
            ValidIssuer = jwtTokenSetup.Issuer,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtTokenSetup.Key)),
            ValidateLifetime = true,
            LifetimeValidator = JwtTokenLifetimeValidator,
        };
    }

    private static string GetNewJwtToken(Claim[] userClaims, JwtTokenSetup jwtTokenSetup)
    {
        var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtTokenSetup.Key));
        var credentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

        var jwtSecToken = new JwtSecurityToken(
            issuer: jwtTokenSetup.Issuer,
            expires: DateTime.UtcNow.AddSeconds(jwtTokenSetup.ExpireSeconds),
            claims: userClaims,
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(jwtSecToken);
    }

    private static byte[] ParseBase64(string base64)
    {
        switch (base64.Length % 4)
        {
            case 2: base64 += "=="; break;
            case 3: base64 += "="; break;
        }
        return Convert.FromBase64String(base64);
    }
}



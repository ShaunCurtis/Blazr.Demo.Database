# Support Classes

#### SessionToken

`SessionToken` is our class that holds our authentication information.

```csharp
public record SessionToken
{
    public Guid SessionId { get; init; } = Guid.NewGuid();
    public string JwtToken { get; init; } = String.Empty;
    public bool IsAuthenticated { get; init; }
    public bool IsEmpty => string.IsNullOrWhiteSpace(this.JwtToken);
}
```

#### IdentityLoginCredentials

`IdentityLoginCredentials` is the class used to pass credentials from the UI to the authentication authority.

```csharp
public class IdentityLoginCredentials
{
    public Guid Id { get; set; }
    public string UserName { get; set; } = String.Empty;
}
```

#### JwtTokenSetup

`JwtTokenSetup` is used to read setup data from `appsettings.json` for thw server running the Jwt Issuer.

```csharp
public class JwtTokenSetup
{
    public string Issuer { get; set; } = String.Empty;
    public string Audience { get; set; } = String.Empty;
    public string Key { get; set; } = String.Empty;
    public int ExpireSeconds { get; set; }
    public int TokenStoreExpireMinutes { get; set; } = 60;
}
```

A typical `appsettings.json`:

```json
  "JwtTokenSetup": {
    "Issuer": "localhost:5167",
    "Audience": "",
    "Key": "ShaunCurtis12345678910HN08CCA12345678910",
    "ExpireSeconds": 15,
    "TokenStoreExpireMinutes": 60
  },
```

#### AuthenticationChangedEventArgs

`AuthenticationChangedEventArgs` is used to pass the new `ClaimsPrincipal` to event listeners when the authentication state changes.

```csharp
public class AuthenticationChangedEventArgs : EventArgs
{
    public ClaimsPrincipal? Identity { get; set; } = null;

    public static AuthenticationChangedEventArgs NewAuthenticationChangedEventArgs(ClaimsPrincipal identity)
        => new AuthenticationChangedEventArgs() { Identity = identity };
}
```
#### SessionTokenManagement

The `SessionTokenManagement` class provides various methods to create manage and validate Jwt Tokens.

```csharp
public static class SessionTokenManagement
{
    public static SessionToken GetNewSessionToken(Claim[] userClaims, JwtTokenSetup jwtTokenSetup)
    {
        var jwtToken = GetNewJwtToken(userClaims, jwtTokenSetup);
        return new SessionToken() { JwtToken = jwtToken, IsAuthenticated = true };
    }

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

    public static bool JwtTokenLifetimeValidator(DateTime? notBefore, DateTime? expires, SecurityToken token, TokenValidationParameters @params)
        => (expires != null)
            ? expires > DateTime.Now
            : false;

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
```

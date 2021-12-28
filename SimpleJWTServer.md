# Simple JWT Issuer Server

The first step is to build a simple JWT Issuer Server.

This will use a set of fixed identities we can use with fixed Guids that are easy to recognise.  The identities are provided by a static `FixedIdentities` object in the Core domain.

DotNetCore's main identity class is `ClaimsPrincipal`, so the class class contains a set of `ClaimsPrincipal` objects for each identity and a set of collections of these objects. 
 
### SimpleIdentities

 `SimpleIdentities` declares three identities: Visitor, User and Admin.  The declaration for `User` along with it's `Claim` collection is shown below.  

```csharp
public static readonly Guid UserId = Guid.Parse("10000000-0000-0000-0000-000000000002");
public static readonly string UserName = "Normal User";

public static ClaimsPrincipal User
    => new ClaimsPrincipal(new ClaimsIdentity(UserClaims, AuthenticationType));

public static Claim[] UserClaims
    => new[]{
            new Claim(ClaimTypes.Sid, UserId.ToString()),
            new Claim(ClaimTypes.Name, UserName),
            new Claim(ClaimTypes.NameIdentifier, "Normal User"),
            new Claim(ClaimTypes.Email, "user@me.com"),
            new Claim(ClaimTypes.Role, "User")
    };
```
The three collections are shown below.

```csharp
    public static Dictionary<string, ClaimsPrincipal> IdentityNameCollection

    public static Dictionary<Guid, ClaimsPrincipal> IdentityIdCollection

    public static Dictionary<Guid, string> IdentityList
```

### SimpleIdentityStore

`SimpleIdentityStore` has one method to get an identity.

```csharp
public class SimpleIdentityStore
{
    public Task<ClaimsPrincipal?> GetIdentityAsync(IdentityLoginCredentials userCredentials)
    {
        TestIdentities.IdentityIdCollection.TryGetValue(userCredentials.Id, out ClaimsPrincipal? principal);
        return Task.FromResult(principal);
    }
}
```

## Data Brokers

### IIdentityDataBroker

```csharp
public interface IIdentityDataBroker
{
    public Task<ClaimsPrincipal?> GetIdentityAsync(IdentityLoginCredentials userCredentials);
}
```

### SimpleIdentityDataBroker

```csharp
public class SimpleIdentityDataBroker : IIdentityDataBroker
{
    private readonly SimpleIdentityStore _simpleIdentityStore;

    public SimpleIdentityDataBroker(SimpleIdentityStore simpleIdentityStore)
        => _simpleIdentityStore = simpleIdentityStore;

    public Task<ClaimsPrincipal?> GetIdentityAsync(IdentityLoginCredentials userCredentials)
        => _simpleIdentityStore.GetIdentityAsync(userCredentials);
}
```

## JWT Issuer

We define an interface:

```csharp
public interface IJwtAuthenticationIssuer
{
    public Task<SessionToken> GetAuthenticationTokenAsync(IdentityLoginCredentials userCredentials);
    public bool TryValidateToken(SessionToken thisSessionToken, out ClaimsPrincipal claimsPrincipal);
    public bool TryValidateToken(SessionToken thisSessionToken);
}
```

Our concrete class is:

```csharp
public class SimpleJwtServerAuthenticationIssuer : IJwtAuthenticationIssuer
{
    private readonly JwtTokenSetup _jwtTokenSetup;
    private readonly IIdentityDataBroker _identityDataBroker;

    public SimpleJwtServerAuthenticationIssuer(IOptions<JwtTokenSetup> jwtTokenSetup, IIdentityDataBroker identityDataBroker)
    {
        _identityDataBroker = identityDataBroker;
        _jwtTokenSetup = jwtTokenSetup!.Value;
    }

    public async Task<SessionToken> GetAuthenticationTokenAsync(IdentityLoginCredentials userCredentials)
    {
        var identity = await _identityDataBroker.GetIdentityAsync(userCredentials);

        return identity is not null && identity.Identity is not null
            ? SessionTokenManagement.GetNewSessionToken(identity!.Claims.ToArray(), _jwtTokenSetup)
            : new SessionToken();
    }

    public bool TryValidateToken(SessionToken thisSessionToken, out ClaimsPrincipal claimsPrincipal)
        => SessionTokenManagement.TryValidateToken(thisSessionToken, _jwtTokenSetup, out claimsPrincipal);

    public bool TryValidateToken(SessionToken thisSessionToken)
        => SessionTokenManagement.TryValidateToken(thisSessionToken, _jwtTokenSetup, out ClaimsPrincipal claimsPrincipal);
}
```

## API

Finally the API controller:

```csharp
[ApiController]
public class JwtAuthenticationController : ControllerBase
{
    private IJwtAuthenticationIssuer _authenticationIssuer;

    public JwtAuthenticationController(IJwtAuthenticationIssuer authenticationIssuer)
        => _authenticationIssuer = authenticationIssuer;

    [AllowAnonymous]
    [Route("/api/authenticate/login-jwttoken")]
    [HttpPost]
    public async Task<IActionResult> GetLoginTokenAsync(IdentityLoginCredentials credentials)
        => this.Ok(await _authenticationIssuer.GetAuthenticationTokenAsync(credentials));

    [AllowAnonymous]
    [Route("/api/authenticate/validate-jwttoken")]
    [HttpPost]
    public IActionResult ValidateToken(SessionToken currentSessionToken)
        => this.Ok(_authenticationIssuer.TryValidateToken(currentSessionToken));
}
```
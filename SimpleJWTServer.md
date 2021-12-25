# Simple JWT Server

`SimpleIdentityStore` provides a set of simple identities we will use for authentication.  It's static: everyone gets the same set of information.  

You can see the full class in the repo.  We'll look at some sections in detail.

DotNetCore's main identity class is `ClaimsPrincipal`. `SimpleIdentityStore` declares three identities: Visitor, User and Admin.  The declaration for `User` along with it's `Claim` collection is shown below.  

```csharp
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
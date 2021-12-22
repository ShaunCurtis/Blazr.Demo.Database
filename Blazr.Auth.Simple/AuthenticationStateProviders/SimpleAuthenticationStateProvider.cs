/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================
namespace Blazr.Auth;

public class SimpleAuthenticationStateProvider : AuthenticationStateProvider
{
    private ClaimsPrincipal _currentIdentity = Anon;

    public static readonly Guid VisitorId = Guid.Parse("10000000-0000-0000-0000-000000000001");
    public static readonly Guid UserId = Guid.Parse("10000000-0000-0000-0000-000000000002");
    public static readonly Guid AdminId = Guid.Parse("10000000-0000-0000-0000-000000000003");

    public static ClaimsPrincipal Anon
        => new ClaimsPrincipal(new ClaimsIdentity(Array.Empty<Claim>(), ""));

    public static ClaimsPrincipal User
        => new ClaimsPrincipal(new ClaimsIdentity(UserClaims, "Simple Auth Type"));

    public static Claim[] UserClaims
        => new[]{
                    new Claim(ClaimTypes.Sid, UserId.ToString()),
                    new Claim(ClaimTypes.Name, "Normal User"),
                    new Claim(ClaimTypes.NameIdentifier, "Normal User"),
                    new Claim(ClaimTypes.Email, "user@me.com"),
                    new Claim(ClaimTypes.Role, "User")
        };

    public static ClaimsPrincipal Visitor
        => new ClaimsPrincipal(new ClaimsIdentity(VisitorClaims, "Simple Auth Type"));

    public static Claim[] VisitorClaims
        => new[]{
                    new Claim(ClaimTypes.Sid, VisitorId.ToString()),
                    new Claim(ClaimTypes.Name, "Visitor"),
                    new Claim(ClaimTypes.NameIdentifier, "Normal Visitor"),
                    new Claim(ClaimTypes.Email, "visitor@me.com"),
                    new Claim(ClaimTypes.Role, "Visitor")
        };

    public static ClaimsPrincipal Admin
        => new ClaimsPrincipal(new ClaimsIdentity(AdminClaims, "Simple Auth Type"));

    public static Claim[] AdminClaims
        => new[]{
                    new Claim(ClaimTypes.Sid, AdminId.ToString()),
                    new Claim(ClaimTypes.Name, "Administrator"),
                    new Claim(ClaimTypes.NameIdentifier, "Administrator"),
                    new Claim(ClaimTypes.Email, "admin@me.com"),
                    new Claim(ClaimTypes.Role, "Admin")
        };


    public override Task<AuthenticationState> GetAuthenticationStateAsync()
        => Task.FromResult(new AuthenticationState(_currentIdentity));

    public Task<AuthenticationState> ChangeIdentity(Guid id)
    {
        var identity = Identities[id];
        _currentIdentity = identity ?? Anon;
        var task = GetAuthenticationStateAsync();
        NotifyAuthenticationStateChanged(task);
        return task;
    }

    public static Dictionary<Guid, ClaimsPrincipal> Identities
        => new Dictionary<Guid, ClaimsPrincipal>()
        {
                {Guid.Empty, Anon},
                {VisitorId, Visitor},
                {UserId, User },
                {AdminId, Admin }
        };

    public static Dictionary<Guid, string> TestIdentities =>
        new Dictionary<Guid, string>()
        {
                {Guid.Empty, "Logged Out" },
                {VisitorId, "Visitor" },
                {UserId, "Normal User" },
                {AdminId, "Admin" }
        };
}


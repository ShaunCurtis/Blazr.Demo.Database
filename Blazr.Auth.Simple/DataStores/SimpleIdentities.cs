namespace Blazr.Auth
{
    public static class SimpleIdentities
    {
        public static readonly Guid VisitorId = Guid.Parse("10000000-0000-0000-0000-000000000001");
        public static readonly Guid UserId = Guid.Parse("10000000-0000-0000-0000-000000000002");
        public static readonly Guid AdminId = Guid.Parse("10000000-0000-0000-0000-000000000003");

        public static readonly string VisitorName = "Visitor";
        public static readonly string UserName = "Normal User";
        public static readonly string AdminName = "Administrator";

        public static ClaimsPrincipal Anon
            => new ClaimsPrincipal(new ClaimsIdentity(Array.Empty<Claim>(), ""));

        public static ClaimsPrincipal User
            => new ClaimsPrincipal(new ClaimsIdentity(UserClaims, "Simple Auth Type"));

        public static Claim[] UserClaims
            => new[]{
                    new Claim(ClaimTypes.Sid, UserId.ToString()),
                    new Claim(ClaimTypes.Name, UserName),
                    new Claim(ClaimTypes.NameIdentifier, "Normal User"),
                    new Claim(ClaimTypes.Email, "user@me.com"),
                    new Claim(ClaimTypes.Role, "User")
            };

        public static ClaimsPrincipal Visitor
            => new ClaimsPrincipal(new ClaimsIdentity(VisitorClaims, "Simple Auth Type"));

        public static Claim[] VisitorClaims
            => new[]{
                    new Claim(ClaimTypes.Sid, VisitorId.ToString()),
                    new Claim(ClaimTypes.Name, VisitorName),
                    new Claim(ClaimTypes.NameIdentifier, "Normal Visitor"),
                    new Claim(ClaimTypes.Email, "visitor@me.com"),
                    new Claim(ClaimTypes.Role, "Visitor")
            };

        public static ClaimsPrincipal Admin
            => new ClaimsPrincipal(new ClaimsIdentity(AdminClaims, "Simple Auth Type"));

        public static Claim[] AdminClaims
            => new[]{
                    new Claim(ClaimTypes.Sid, AdminId.ToString()),
                    new Claim(ClaimTypes.Name, AdminName),
                    new Claim(ClaimTypes.NameIdentifier, "Administrator"),
                    new Claim(ClaimTypes.Email, "admin@me.com"),
                    new Claim(ClaimTypes.Role, "Admin")
            };

        public static Dictionary<string, ClaimsPrincipal> IdentityCollection
            => new Dictionary<string, ClaimsPrincipal>()
            {
                        {VisitorName, Visitor},
                        {UserName, User },
                        {AdminName, Admin }
            };

        public static Dictionary<Guid, string> IdentityList =>
            new Dictionary<Guid, string>()
            {
                {Guid.Empty, "Logged Out" },
                {VisitorId, VisitorName },
                {UserId, UserName },
                {AdminId, AdminName }
            };

        public static bool TryGetIdentity(IdentityLoginCredentials userCredentials, out ClaimsPrincipal identity)
        {
            var result = IdentityCollection[userCredentials.UserName];
            identity = result ?? Anon;
            return result is not null;
        }
    }
}

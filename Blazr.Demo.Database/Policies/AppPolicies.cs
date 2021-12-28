/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================


namespace Blazr.Demo.Database
{
    public static class AppPolicies
    {
        public const string IsAdmin = "IsAdmin";
        public const string IsUser = "IsUser";
        public const string IsVisitor = "IsVisitor";

        public static AuthorizationPolicy IsAdminPolicy
            => new AuthorizationPolicyBuilder()
            .RequireAuthenticatedUser()
            .RequireRole("Admin")
            .Build();

        public static AuthorizationPolicy IsUserPolicy
            => new AuthorizationPolicyBuilder()
            .RequireAuthenticatedUser()
            .RequireRole("Admin", "User")
            .Build();

        public static AuthorizationPolicy IsVisitorPolicy
            => new AuthorizationPolicyBuilder()
            .RequireAuthenticatedUser()
            .RequireRole("Admin", "User", "Visitor")
            .Build();
    }
}

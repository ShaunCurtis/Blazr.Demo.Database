/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================

using Microsoft.Extensions.DependencyInjection;
using Blazr.Auth.JWT.Simple.Core;
using Blazr.Auth.JWT.Simple.Data;

namespace Blazr.Auth.JWT.Simple;

public static class ServiceCollectionExtensions
{
    public static void AddSimpleAuthentication(this IServiceCollection services)
    {
        services.AddScoped<AuthenticationStateProvider, DumbAuthenticationStateProvider>();
        services.AddAuthorizationCore();
    }

    public static void AddSimpleJwtWASMAuthentication(this IServiceCollection services)
    {
        services.AddScoped<IClientAuthenticationService, SimpleJwtClientAuthenticationService>();
        services.AddScoped<AuthenticationStateProvider, ServiceAuthenticationStateProvider>();
        services.AddAuthorizationCore(config =>
        {
            config.AddPolicy(AppPolicies.IsAdmin, AppPolicies.IsAdminPolicy);
            config.AddPolicy(AppPolicies.IsUser, AppPolicies.IsUserPolicy);
            config.AddPolicy(AppPolicies.IsVisitor, AppPolicies.IsVisitorPolicy);
        });
    }
}


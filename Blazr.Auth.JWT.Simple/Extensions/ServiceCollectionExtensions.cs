/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================

using Blazr.Auth.JWT.Simple.Core;
using Microsoft.Extensions.DependencyInjection;

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
            foreach (var policy in AppPolicies.Policies)
            {
                config.AddPolicy(policy.Key, policy.Value);
            }
        });
    }
}


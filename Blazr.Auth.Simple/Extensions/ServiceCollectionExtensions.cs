/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================

using Microsoft.Extensions.DependencyInjection;
using Blazr.Auth.Core;
using Blazr.Auth.Data;

namespace Blazr.Auth;

public static class ServiceCollectionExtensions
{
    public static void AddSimpleAuthentication(this IServiceCollection services)
    {
        services.AddScoped<AuthenticationStateProvider, SimpleAuthenticationStateProvider>();
        services.AddAuthorizationCore();
    }

    public static void AddSimpleJwtAuthentication(this IServiceCollection services)
    {
        services.AddSingleton<SimpleIdentityStore>();
        services.AddSingleton<IIdentityDataBroker, SimpleIdentityDataBroker>();
        services.AddSingleton<IJwtAuthenticationIssuer, SimpleJwtServerAuthenticationIssuer>();
        services.AddScoped<IClientAuthenticationService, SimpleJwtServerClientAuthenticationService>();
        services.AddScoped<AuthenticationStateProvider, SimpleJwtAuthenticationStateProvider>();
        //services.AddAuthorizationCore();
    }
    public static void AddWASMServerSimpleJwtAuthentication(this IServiceCollection services)
    {
        services.AddScoped<AuthenticationStateProvider, SimpleJwtAuthenticationStateProvider>();
        services.AddScoped<IClientAuthenticationService, SimpleJwtServerClientAuthenticationService>();
        services.AddSingleton<IJwtAuthenticationIssuer, SimpleJwtServerAuthenticationIssuer>();
    }
}


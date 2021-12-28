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

    public static void AddSimpleJwtServerAuthentication(this IServiceCollection services)
    {
        services.AddSingleton<SimpleIdentityStore>();
        services.AddSingleton<IIdentityDataBroker, SimpleIdentityDataBroker>();
        services.AddSingleton<IJwtAuthenticationIssuer, SimpleJwtServerAuthenticationIssuer>();
        services.AddScoped<IClientAuthenticationService, SimpleJwtServerClientAuthenticationService>();
        services.AddScoped<AuthenticationStateProvider, ServiceAuthenticationStateProvider>();
    }

    public static void AddSimpleJwtWASMAuthentication(this IServiceCollection services)
    {
        services.AddScoped<IClientAuthenticationService, SimpleJwtClientAuthenticationService>();
        services.AddScoped<AuthenticationStateProvider, ServiceAuthenticationStateProvider>();
    }

    public static void AddSimpleJwtWASMServerAuthentication(this IServiceCollection services)
    {
        services.AddSingleton<SimpleIdentityStore>();
        services.AddSingleton<IIdentityDataBroker, SimpleIdentityDataBroker>();
        services.AddSingleton<IJwtAuthenticationIssuer, SimpleJwtServerAuthenticationIssuer>();
        //services.AddScoped<AuthenticationStateProvider, ServiceAuthenticationStateProvider>();
    }
}


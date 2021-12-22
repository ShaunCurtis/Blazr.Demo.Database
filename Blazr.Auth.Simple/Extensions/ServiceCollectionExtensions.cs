/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================

using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore;

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
        services.AddScoped<AuthenticationStateProvider, SimpleJwtAuthenticationStateProvider>();
        services.AddScoped<IClientAuthenticationService, SimpleJwtServerClientAuthenticationService>();
        services.AddSingleton<IJwtAuthenticationIssuer, SimpleJwtServerAuthenticationIssuer>();
        services.AddBlazoredLocalStorage();
        //services.AddAuthorizationCore();
    }
}


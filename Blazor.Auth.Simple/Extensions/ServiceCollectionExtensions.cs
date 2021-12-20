/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================

using Blazor.Auth.Simple.AuthenticationStateProviders;
using Microsoft.Extensions.DependencyInjection;

namespace Blazor.Auth.Simple.Extensions;

public static class ServiceCollectionExtensions
{

    public static void AddSimpleAuthentication(this IServiceCollection services)
    {
        services.AddScoped<AuthenticationStateProvider, SimpleAuthenticationStateProvider>();
        services.AddAuthorizationCore();
    }

}


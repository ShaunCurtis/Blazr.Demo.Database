/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================


using Blazr.Auth.JWT.Simple;
using Blazr.Core.Toaster;
using Blazr.Routing;

namespace Blazr.Demo.Database.API.Web;

public static class IServiceCollectionExtensions
{
    public static void AddAppBlazorServerServices(this WebApplicationBuilder builder)
    {
        var services = builder.Services;
        services.AddSingleton<WeatherForecastDataStore>();
        services.AddSingleton<IWeatherForecastDataBroker, WeatherForecastServerDataBroker>();
        services.AddScoped<WeatherForecastsViewService>();
        services.AddTransient<WeatherForecastViewService>();
        services.AddBlazrNavigationManager();
        services.AddSingleton<ResponseMessageStore>();
        services.AddSingleton<ToasterService>();
    }

    public static void AddAppWASMServerServices(this WebApplicationBuilder builder)
    {
        var services = builder.Services;
        services.AddControllers().PartManager.ApplicationParts.Add(new AssemblyPart(typeof(Blazr.Demo.Database.API.Web.WeatherForecastController).Assembly));
        services.AddSingleton<WeatherForecastDataStore>();
        services.AddSingleton<IWeatherForecastDataBroker, WeatherForecastServerDataBroker>();
    }

    public static void AddAppBlazorServerAuthorizationServices(this WebApplicationBuilder builder)
    {
        var services = builder.Services;
        services.AddSingleton<IAuthorizationHandler, WeatherForecastOwnerAuthorizationHandler>();
        services.AddSingleton<IAuthorizationHandler, WeatherForecastEditorAuthorizationHandler>();
        services.AddAuthorization(config =>
        {
            foreach (var policy in SimpleJWTPolicies.Policies)
            {
                config.AddPolicy(policy.Key, policy.Value);
            }

            foreach (var policy in AppPolicies.Policies)
            {
                config.AddPolicy(policy.Key, policy.Value);
            }
        });

    }


}


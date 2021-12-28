/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================


namespace Blazr.Demo.Database;

public static class IServiceCollectionExtensions
{
    public static void AddAppBlazorServerServices(this IServiceCollection services)
    {
        services.AddSingleton<WeatherForecastDataStore>();
        services.AddSingleton<IWeatherForecastDataBroker, WeatherForecastServerDataBroker>();
        services.AddScoped<WeatherForecastsViewService>();
        services.AddTransient<WeatherForecastViewService>();
        services.AddBlazrNavigationLockerServerServices();
    }

    public static void AddAppBlazorWASMServices(this IServiceCollection services)
    {
        services.AddScoped<IWeatherForecastDataBroker, WeatherForecastAPIDataBroker>();
        services.AddScoped<WeatherForecastViewService>();
        services.AddTransient<WeatherForecastsViewService>();
        services.AddBlazrNavigationLockerWASMServices();
        services.AddAuthorizationCore(config =>
        {
            config.AddPolicy(AppPolicies.IsAdmin, AppPolicies.IsAdminPolicy);
            config.AddPolicy(AppPolicies.IsUser, AppPolicies.IsUserPolicy);
            config.AddPolicy(AppPolicies.IsVisitor, AppPolicies.IsVisitorPolicy);
        });
    }

    public static void AddAppWASMServerServices(this IServiceCollection services)
    {
        services.AddSingleton<WeatherForecastDataStore>();
        services.AddSingleton<IWeatherForecastDataBroker, WeatherForecastServerDataBroker>();
    }
}


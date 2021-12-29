﻿/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================
namespace Blazr.Demo.Database;

public static class IServiceCollectionExtensions
{
    public static void AddAppBlazorWASMServices(this IServiceCollection services)
    {
        services.AddScoped<IWeatherForecastDataBroker, WeatherForecastAPIDataBroker>();
        services.AddScoped<WeatherForecastViewService>();
        services.AddTransient<WeatherForecastsViewService>();
        services.AddBlazrNavigationLockerWASMServices();
        services.AddSingleton<ResponseMessageStore>();
    }
}


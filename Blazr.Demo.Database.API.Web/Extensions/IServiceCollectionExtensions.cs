/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================


using Blazr.Core.Toaster;

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
        services.AddBlazrNavigationLockerServerServices();
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
}


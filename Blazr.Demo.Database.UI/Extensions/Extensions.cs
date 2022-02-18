/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================

namespace Blazr.Demo.Database.UI;

public static class Extensions
{
    public static int TemperatureF(this DvoWeatherForecast forecast)
        => 32 + (int)(forecast.TemperatureC / 0.5556);

    public static int TemperatureF(this DcoWeatherForecast forecast)
        => 32 + (int)(forecast.TemperatureC / 0.5556);

    public static int TemperatureF(this DeoWeatherForecast forecast)
        => 32 + (int)(forecast.TemperatureC / 0.5556);
}


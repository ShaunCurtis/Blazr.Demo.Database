/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================

namespace Blazr.Demo.Database.Core
{
    public record DvoWeatherForecast
    {
        public Guid WeatherForecastId { get; init; } = GuidExtensions.Null;

        public Guid OwnerId { get; init; }

        public string OwnerName { get; init; } = String.Empty;

        public DateTime Date { get; init; }

        public int TemperatureC { get; init; }

        public string Summary { get; init; } = String.Empty;
    }
}

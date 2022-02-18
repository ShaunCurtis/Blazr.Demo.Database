/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================

namespace Blazr.Demo.Database.Core
{
    public class DeoWeatherForecast
    {
        public Guid WeatherForecastId { get; set; } = GuidExtensions.Null;

        public Guid OwnerId { get; set; }

        public DateTime Date { get; set; }

        public int TemperatureC { get; set; }

        public string Summary { get; set; } = String.Empty;

        public void Populate(DcoWeatherForecast record)
        {
            this.WeatherForecastId = record.WeatherForecastId;
            this.OwnerId = record.OwnerId;
            this.Date = record.Date;
            this.Summary = record.Summary;
            this.TemperatureC = record.TemperatureC;
        }

        public DcoWeatherForecast ToDco =>
            new DcoWeatherForecast() {
                WeatherForecastId = this.WeatherForecastId,
                OwnerId = this.OwnerId,
                Date = this.Date,
                Summary = this.Summary,
                TemperatureC = this.TemperatureC
            };
    }
}

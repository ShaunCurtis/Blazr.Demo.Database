/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================

namespace Blazr.Demo.Database.Core
{
    public class DeoWeatherForecast
    {
        public Guid Id { get; set; } = GuidExtensions.Null;

        public Guid OwnerId { get; set; }

        public DateTime Date { get; set; }

        public int TemperatureC { get; set; }

        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        public string Summary { get; set; } = String.Empty;

        public bool IsNull => Id == GuidExtensions.Null;

        public void Populate(DcoWeatherForecast record)
        {
            this.Id = record.Id;
            this.OwnerId = record.OwnerId;
            this.Date = record.Date;
            this.Summary = record.Summary;
            this.TemperatureC = record.TemperatureC;
        }

        public DcoWeatherForecast ToDco =>
            new DcoWeatherForecast() {
                Id = this.Id,
                OwnerId = this.OwnerId,
                Date = this.Date,
                Summary = this.Summary,
                TemperatureC = this.TemperatureC
            };
    }
}

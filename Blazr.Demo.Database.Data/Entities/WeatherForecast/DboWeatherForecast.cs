/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================
namespace Blazr.Demo.Database.Data
{
    internal record DboWeatherForecast
    {
        [Key]
        public Guid Id { get; init; }

        public Guid OwnerId { get; init; }

        public DateTime Date { get; init; }

        public int TemperatureC { get; init; }

        public string? Summary { get; init; }

        public DcoWeatherForecast ToDto()
            => new DcoWeatherForecast
            {
                Id = this.Id,
                OwnerId = this.OwnerId,
                Date = this.Date,
                TemperatureC = this.TemperatureC,
                Summary = this.Summary ?? string.Empty,
            };

        public static DboWeatherForecast FromDto(DcoWeatherForecast record)
            => new DboWeatherForecast
            {
                Id = record.Id,
                OwnerId = record.OwnerId, 
                Date = record.Date,
                TemperatureC = record.TemperatureC,
                Summary = record.Summary
            };
    }
}

/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================

namespace Blazr.Demo.Database.Core
{
    public record DcoWeatherForecast
    {
        public Guid Id { get; init; } = GuidExtensions.Null;

        public Guid OwnerId { get; init; }

        public DateTime Date { get; init; }

        public int TemperatureC { get; init; }

        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        public string Summary { get; init; } = String.Empty;

        public bool IsNull => Id == GuidExtensions.Null;
    }
}

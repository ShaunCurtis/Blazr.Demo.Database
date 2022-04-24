/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================


namespace Blazr.Demo.Database.Core;

public class DeoWeatherLocation : IDeoRecord<DboWeatherLocation>
{
    [Key]
    public Guid WeatherLocationId { get; set; } = GuidExtensions.Null;

    public string Name { get; set; } = String.Empty;

    public Guid Id => this.WeatherLocationId;
  
    public bool IsNull => Id == GuidExtensions.Null;

    public void Populate(DboWeatherLocation record)
    {
        this.WeatherLocationId = record.WeatherLocationId;
        this.Name = record.Name ?? String.Empty;
    }

    public DboWeatherLocation ToDbo() =>
        new DboWeatherLocation()
        {
            WeatherLocationId = this.WeatherLocationId,
            Name = this.Name
        };
}


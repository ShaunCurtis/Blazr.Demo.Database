/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================

using System.ComponentModel.DataAnnotations;

namespace Blazr.Demo.Database.Core;

public record DboWeatherLocation
{
    [Key]
    public Guid WeatherLocationId { get; init; }

    public string? Name { get; init; }

    public Guid Id => WeatherLocationId;
}


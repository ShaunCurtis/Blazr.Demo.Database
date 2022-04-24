/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================

namespace Blazr.Demo.Database.Core;

public class WeatherLocationListViewService :
    BaseListViewService<DboWeatherLocation>, IListViewService<DboWeatherLocation>
{
    public WeatherLocationListViewService(IListDataBroker weatherDataBroker) : base(weatherDataBroker) {  }
}


/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================

namespace Blazr.Demo.Database.Core;

public class WeatherLocationCrudViewService : 
    BaseCrudViewService<DboWeatherLocation, DeoWeatherLocation, DboWeatherLocation>
{
    public WeatherLocationCrudViewService(ICrudDataBroker crudDataBroker, 
        IListViewService<DboWeatherLocation> viewService, 
        ResponseMessageStore responseMessageStore) : 
        base(crudDataBroker, viewService, responseMessageStore)
    { }
}

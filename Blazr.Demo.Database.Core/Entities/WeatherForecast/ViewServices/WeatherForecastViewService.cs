/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================
namespace Blazr.Demo.Database.Core;

public class WeatherForecastViewService
{
    private readonly IWeatherForecastDataBroker? weatherForecastDataBroker;

    private readonly WeatherForecastsViewService weatherForecastsViewService;

    private readonly ResponseMessageStore responseMessageStore;

    public DcoWeatherForecast Record { get; private set; } = new DcoWeatherForecast();

    public DeoWeatherForecast EditModel { get; private set; } = new DeoWeatherForecast();

    public WeatherForecastViewService(IWeatherForecastDataBroker weatherForecastDataBroker, WeatherForecastsViewService weatherForecastsViewService, ResponseMessageStore responseMessageStore)
    {
        this.weatherForecastDataBroker = weatherForecastDataBroker!;
        this.weatherForecastsViewService = weatherForecastsViewService;
        this.responseMessageStore = responseMessageStore;
    }

    public async ValueTask GetForecastAsync(Guid Id)
    {
        this.Record = await weatherForecastDataBroker!.GetForecastAsync(Guid.NewGuid(), Id);
        this.EditModel.Populate(this.Record);
    }

    public async ValueTask AddRecordAsync(Guid transactionId, DcoWeatherForecast record)
    {
        this.Record = record;
        await weatherForecastDataBroker!.AddForecastAsync(transactionId, this.Record);
        weatherForecastsViewService.NotifyListChanged(this, EventArgs.Empty);
    }

    public async ValueTask UpdateRecordAsync(Guid transactionId)
    {
        this.Record = EditModel.ToDco;
        await weatherForecastDataBroker!.UpdateForecastAsync(transactionId, this.Record);
        weatherForecastsViewService.NotifyListChanged(this, EventArgs.Empty);

    }

    public async ValueTask DeleteRecordAsync(Guid transactionId, Guid Id)
    {
        _ = await weatherForecastDataBroker!.DeleteForecastAsync(transactionId, Id);
        weatherForecastsViewService.NotifyListChanged(this, EventArgs.Empty);
    }
}

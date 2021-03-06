/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================
namespace Blazr.Demo.Database.Core;

public class WeatherForecastCrudService
{
    private readonly IWeatherForecastDataBroker? weatherForecastDataBroker;
    private readonly WeatherForecastListService weatherForecastsViewService;
    private readonly WeatherForecastNotificationService weatherForecastNotificationService;
    private readonly ResponseMessageStore responseMessageStore;

    public DcoWeatherForecast Record { get; private set; } = new DcoWeatherForecast();

    public DeoWeatherForecast EditModel { get; private set; } = new DeoWeatherForecast();

    public WeatherForecastCrudService(IWeatherForecastDataBroker weatherForecastDataBroker, WeatherForecastListService weatherForecastsViewService, ResponseMessageStore responseMessageStore, WeatherForecastNotificationService weatherForecastNotificationService)
    {
        this.weatherForecastDataBroker = weatherForecastDataBroker!;
        this.weatherForecastsViewService = weatherForecastsViewService;
        this.responseMessageStore = responseMessageStore;
        this.weatherForecastNotificationService = weatherForecastNotificationService;
    }

    public async ValueTask GetForecastAsync(Guid Id)
    {
        this.Record = await weatherForecastDataBroker!.GetForecastAsync(Guid.NewGuid(), Id);
        this.EditModel.Populate(this.Record);
    }

    public async ValueTask AddRecordAsync(Guid transactionId, DcoWeatherForecast record)
    {
        this.Record = record;
        if (await weatherForecastDataBroker!.AddForecastAsync(transactionId, this.Record))
            responseMessageStore.AddMessage(new ResponseMessage {Id = transactionId, Message = "Record Added" });
        else
            responseMessageStore.AddMessage(new ResponseMessage { Id = transactionId, Message = "Record Add failed", OK = false });

        this.weatherForecastNotificationService.NotifyRecordChanged(this, new RecordChangedEventArgs());
        this.weatherForecastNotificationService.NotifyRecordSetChanged(this, new RecordSetChangedEventArgs());
    }

    public async ValueTask UpdateRecordAsync(Guid transactionId)
    {
        this.Record = EditModel.ToDco;
        if (await weatherForecastDataBroker!.UpdateForecastAsync(transactionId, this.Record))
            responseMessageStore.AddMessage(new ResponseMessage { Id = transactionId, Message = "Record Updated" });
        else
            responseMessageStore.AddMessage(new ResponseMessage { Id = transactionId, Message = "Record Update failed", OK = false });

        this.weatherForecastNotificationService.NotifyRecordChanged(this, new RecordChangedEventArgs());
        this.weatherForecastNotificationService.NotifyRecordSetChanged(this, new RecordSetChangedEventArgs());
    }

    public async ValueTask DeleteRecordAsync(Guid transactionId, Guid Id)
    {
        if (await weatherForecastDataBroker!.DeleteForecastAsync(transactionId, Id))
            responseMessageStore.AddMessage(new ResponseMessage { Id = transactionId, Message = "Record deleted" });
        else
            responseMessageStore.AddMessage(new ResponseMessage { Id = transactionId, Message = "Record Delete failed", OK = false });

        this.weatherForecastNotificationService.NotifyRecordChanged(this, new RecordChangedEventArgs());
        this.weatherForecastNotificationService.NotifyRecordSetChanged(this, new RecordSetChangedEventArgs());
    }
}

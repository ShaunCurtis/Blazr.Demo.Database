/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================

namespace Blazr.Demo.Database.Core;

public class WeatherForecastListService
{
    private readonly IWeatherForecastDataBroker weatherForecastDataBroker;
    private readonly WeatherForecastNotificationService weatherForecastNotificationService;

    public List<DvoWeatherForecast>? Records { get; private set; }

    public int RecordCount { get; private set; } = 0;

    public WeatherForecastListService(IWeatherForecastDataBroker weatherForecastDataBroker, WeatherForecastNotificationService weatherForecastNotificationService)
    { 
        this.weatherForecastDataBroker = weatherForecastDataBroker;
        this.weatherForecastNotificationService = weatherForecastNotificationService;
    }

    public async ValueTask<ListOptions> GetForecastsAsync(ListOptions request)
    {
        var isfirstLoad = this.Records is null || this.Records.Count == 0;

        this.Records = await weatherForecastDataBroker!.GetWeatherForecastsAsync(Guid.NewGuid(), request);
        this.RecordCount = await weatherForecastDataBroker.GetWeatherForecastCountAsync(Guid.NewGuid());
        if (isfirstLoad)
            this.weatherForecastNotificationService.NotifyRecordSetChanged(this, new RecordSetChangedEventArgs());
        return request.GetCopy(this.RecordCount);
    }

    public async ValueTask<ItemsProviderResult<DvoWeatherForecast>> GetForecastsAsync(ItemsProviderRequest request)
    {
        var isfirstLoad = this.Records is null || this.Records.Count == 0;
        var options = new ListOptions { PageSize = request.Count, StartRecord = request.StartIndex};

        this.Records = await weatherForecastDataBroker!.GetWeatherForecastsAsync(Guid.NewGuid(), options);
        this.RecordCount = await weatherForecastDataBroker.GetWeatherForecastCountAsync(Guid.NewGuid());
        if (isfirstLoad)
            this.weatherForecastNotificationService.NotifyRecordSetChanged(this, new RecordSetChangedEventArgs());
        return new ItemsProviderResult<DvoWeatherForecast>(this.Records, this.RecordCount);
    }
}


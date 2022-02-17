/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================

namespace Blazr.Demo.Database.Core;

public class WeatherForecastsViewService
{
    private readonly IWeatherForecastDataBroker weatherForecastDataBroker;
    private readonly WeatherForecastNotificationService weatherForecastNotificationService;

    public List<DcoWeatherForecast>? Records { get; private set; }

    public readonly ListOptions ListOptions = new ListOptions() { PageSize=1000 };

    public event EventHandler<EventArgs>? ListChanged;

    public int RecordCount { get; private set; } = 0;

    public WeatherForecastsViewService(IWeatherForecastDataBroker weatherForecastDataBroker, WeatherForecastNotificationService weatherForecastNotificationService)
    { 
        this.weatherForecastDataBroker = weatherForecastDataBroker;
        this.weatherForecastNotificationService = weatherForecastNotificationService;
    }

    public async ValueTask GetForecastsAsync()
    {
        this.Records = null;
        this.ListChanged?.Invoke(this.Records, EventArgs.Empty);
        this.Records = await weatherForecastDataBroker.GetWeatherForecastsAsync(Guid.NewGuid(), this.ListOptions);
        this.ListChanged?.Invoke(this.Records, EventArgs.Empty);
    }
    public async ValueTask<ItemsProviderResult<DcoWeatherForecast>> GetForecastsAsync(ItemsProviderRequest request)
    {
        var isfirstLoad = false;
        if (this.Records is null || this.Records.Count == 0)
            isfirstLoad = true;

        this.ListOptions.StartRecord = request.StartIndex;
        this.ListOptions.PageSize = request.Count;
        this.Records = await weatherForecastDataBroker!.GetWeatherForecastsAsync(Guid.NewGuid(), this.ListOptions);
        this.RecordCount = await weatherForecastDataBroker.GetWeatherForecastCountAsync(Guid.NewGuid());
        if (isfirstLoad)
            this.weatherForecastNotificationService.NotifyRecordSetChanged(this, new RecordSetChangedEventArgs());
        return new ItemsProviderResult<DcoWeatherForecast>(this.Records, this.RecordCount);
    }
}


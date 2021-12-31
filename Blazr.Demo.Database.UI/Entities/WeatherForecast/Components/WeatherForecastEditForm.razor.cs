/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================


namespace Blazr.Demo.Database.UI;

public partial class WeatherForecastEditForm : BaseEditForm, IDisposable
{
    private WeatherForecastViewService viewService => _viewService!;
    private WeatherForecastViewService? _viewService;

    protected async override Task OnInitializedAsync()
    {
        _viewService = ScopedServices.GetService(typeof(WeatherForecastViewService)) as WeatherForecastViewService;

        base.LoadState = ComponentState.Loading;
        await this.viewService.GetForecastAsync(Id);
        base.editContent = new EditContext(this.viewService.EditModel);
        base.editStateContext = new EditStateContext(base.editContent);
        base.editStateContext.EditStateChanged += base.OnEditStateChanged;
        base.LoadState = ComponentState.Loaded;
    }

    private async Task SaveRecord()
    {
        var tranactionId = Guid.NewGuid();
        await this.viewService.UpdateRecordAsync(tranactionId);
        base.editStateContext?.NotifySaved();
    }

    private async Task AddRecord()
    {
        var tranactionId = Guid.NewGuid();
        await this.viewService.AddRecordAsync(tranactionId,
            new DcoWeatherForecast
            {
                Date = DateTime.Now,
                Id = Guid.NewGuid(),
                Summary = "Balmy",
                TemperatureC = 14
            });
    }

    protected override void BaseExit()
    => this.NavManager?.NavigateTo("/weatherforecast");

    public void Dispose()
    {
        if (base.editStateContext is not null)
            base.editStateContext.EditStateChanged -= base.OnEditStateChanged;
    }
}

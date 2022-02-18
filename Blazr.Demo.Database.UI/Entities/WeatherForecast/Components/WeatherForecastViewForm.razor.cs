/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================


namespace Blazr.Demo.Database.UI;

public partial class WeatherForecastViewForm : BaseViewForm<DcoWeatherForecast>
{
    private WeatherForecastCrudService? _viewService;

    private WeatherForecastCrudService viewService => this._viewService!;

    protected async override Task OnInitializedAsync()
    {
        _viewService = ScopedServices.GetService(typeof(WeatherForecastCrudService)) as WeatherForecastCrudService;

        base.LoadState = ComponentState.Loading;
        await this.viewService.GetForecastAsync(Id);

        if (!await this.CheckAuthorization(new AppAuthFields { OwnerId = this.viewService.Record.OwnerId }, AppPolicies.IsViewer))
        {
            LoadState = ComponentState.UnAuthorized;
            return;
        }

        base.LoadState = ComponentState.Loaded;
    }

    protected override void BaseExit()
    => this.NavManager?.NavigateTo("/weatherforecast");
}

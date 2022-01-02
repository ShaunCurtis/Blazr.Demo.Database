/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================


namespace Blazr.Demo.Database.UI;

public partial class WeatherForecastViewForm : BaseViewForm<DcoWeatherForecast>
{
    private WeatherForecastViewService? _viewService;

    private WeatherForecastViewService viewService => this._viewService!;

    protected async override Task OnInitializedAsync()
    {
        _viewService = ScopedServices.GetService(typeof(WeatherForecastViewService)) as WeatherForecastViewService;

        base.LoadState = ComponentState.Loading;
        await this.viewService.GetForecastAsync(Id);

        if (!await this.CheckAuthorization(this.viewService.Record, AppPolicies.IsViewer))
        {
            LoadState = ComponentState.UnAuthorized;
            return;
        }

        base.LoadState = ComponentState.Loaded;
    }

    protected override void BaseExit()
    => this.NavManager?.NavigateTo("/weatherforecast");
}

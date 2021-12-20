/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================


namespace Blazr.Demo.Database.UI;

public partial class WeatherForecastViewForm : BaseViewForm
{
    [Inject] private WeatherForecastViewService? ViewService { get; set; }

    private WeatherForecastViewService viewService => this.ViewService!;

    protected async override Task OnInitializedAsync()
    {
        base.LoadState = ComponentState.Loading;
        await this.viewService.GetForecastAsync(Id);
        base.LoadState = ComponentState.Loaded;
    }

    protected override void BaseExit()
    => this.NavManager?.NavigateTo("/weatherforecast");
}

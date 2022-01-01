/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================

using Blazr.Core.Toaster;

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
        var transactionId = Guid.NewGuid();
        await this.viewService.UpdateRecordAsync(transactionId);
        base.editStateContext?.NotifySaved();

        var message = ResponseMessageStore?.GetMessage(transactionId);
        if (message is null)
            return;

        if (message.OK)
            toasterService?.AddToast(Toast.NewTTD("Save", $"Record Saved.", MessageColour.Success, 5));
        else
            toasterService?.AddToast(Toast.NewTTD("Save", $"Save Failed.  Server response: {message.Message}", MessageColour.Danger, 30));
    }

    private async Task AddRecord()
    {
        var transactionId = Guid.NewGuid();
        await this.viewService.AddRecordAsync(transactionId,
            new DcoWeatherForecast
            {
                Date = DateTime.Now,
                Id = Guid.NewGuid(),
                Summary = "Balmy",
                TemperatureC = 14
            });

        var message = ResponseMessageStore?.GetMessage(transactionId);
        if (message is null)
            return;

        if (message.OK)
            toasterService?.AddToast(Toast.NewTTD("Add", $"Record Added.", MessageColour.Success, 5));
        else
            toasterService?.AddToast(Toast.NewTTD("Add", $"Add Failed.  Server response: {message.Message}", MessageColour.Danger, 30));
    }

    protected override void BaseExit()
    => this.NavManager?.NavigateTo("/weatherforecast");

    public void Dispose()
    {
        if (base.editStateContext is not null)
            base.editStateContext.EditStateChanged -= base.OnEditStateChanged;
    }
}

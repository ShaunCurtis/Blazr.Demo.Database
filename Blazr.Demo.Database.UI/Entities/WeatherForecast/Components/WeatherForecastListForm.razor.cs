using Blazr.Core.Toaster;

namespace Blazr.Demo.Database.UI;

public partial class WeatherForecastListForm : ComponentBase
{
    [Inject] private WeatherForecastsViewService? listViewService { get; set; }

    [Inject] private WeatherForecastViewService? recordViewService { get; set; }

    [Inject] private NavigationManager? navigationManager { get; set; }

    [Inject] private ToasterService? toasterService { get; set; }

    [Inject] private ResponseMessageStore? ResponseMessageStore { get; set; }

    private bool isLoading => listViewService!.Records is null;

    public bool IsModal { get; set; }

    private IModalDialog? modalDialog { get; set; }

    private ComponentState loadState => isLoading ? ComponentState.Loading : ComponentState.Loaded;

    protected async override Task OnInitializedAsync()
    {
        await this.listViewService!.GetForecastsAsync();
        this.listViewService.ListChanged += this.OnListChanged;
    }

    private async Task DeleteRecord(Guid Id)
    {
        var transactionId = Guid.NewGuid();
        await this.recordViewService!.DeleteRecordAsync(transactionId, Id);

        var message = ResponseMessageStore?.GetMessage(transactionId);
        if (message is not null)
        {
            if (message.OK)
                toasterService?.AddToast(Toast.NewTTD("Delete", $"Record Deleted.", MessageColour.Success, 5));
            else
                toasterService?.AddToast(Toast.NewTTD("Delete", $"Delete Failed.  Server response: {message.Message}", MessageColour.Danger, 30));
        }
    }

    private async Task EditRecord(Guid Id)
    {
        if (this.IsModal)
        {
            var options = new ModalOptions();
            options.Set("Id", Id);
            await this.modalDialog!.ShowAsync<WeatherForecastEditForm>(options);
        }
        else
            this.navigationManager!.NavigateTo($"/WeatherForecast/Edit/{Id}");
    }

    private async Task ViewRecord(Guid Id)
    {
        if (this.IsModal)
        {
            var options = new ModalOptions();
            options.Set("Id", Id);
            await this.modalDialog!.ShowAsync<WeatherForecastViewForm>(options);
        }
        else
            this.navigationManager!.NavigateTo($"/WeatherForecast/View/{Id}");
    }

    private async Task AddRecordAsync()
    {
        var transactionId = Guid.NewGuid();
        var record = new DcoWeatherForecast
        {
            Date = DateTime.Now,
            Id = Guid.NewGuid(),
            Summary = "Balmy",
            TemperatureC = 14
        };
        await this.recordViewService!.AddRecordAsync(transactionId, record);

        var message = ResponseMessageStore?.GetMessage(transactionId);
        if (message is not null)
        {
            if (message.OK)
                toasterService?.AddToast(Toast.NewTTD("Add", $"Record added.", MessageColour.Success, 5));
            else
                toasterService?.AddToast(Toast.NewTTD("Add", $"Add Failed.  Server response: {message.Message}", MessageColour.Danger, 30));
        }
    }

    private void OnListChanged(object? sender, EventArgs e)
        => this.InvokeAsync(this.StateHasChanged);

    public void Dispose()
        => this.listViewService!.ListChanged -= this.OnListChanged;
}


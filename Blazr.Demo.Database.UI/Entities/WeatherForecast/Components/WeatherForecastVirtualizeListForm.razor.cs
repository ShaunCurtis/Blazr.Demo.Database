using Microsoft.AspNetCore.Components.Web.Virtualization;
/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================

namespace Blazr.Demo.Database.UI;

public partial class WeatherForecastVirtualizeListForm : ComponentBase
{
    [CascadingParameter] public Task<AuthenticationState>? AuthTask { get; set; }

    [Inject] private WeatherForecastsViewService? _listViewService { get; set; }

    [Inject] private WeatherForecastViewService? _recordViewService { get; set; }

    [Inject] private WeatherForecastNotificationService? _notificationService { get; set; }

    [Inject] private NavigationManager? navigationManager { get; set; }

    [Inject] private ToasterService? toasterService { get; set; }

    [Inject] private ResponseMessageStore? ResponseMessageStore { get; set; }

    [Inject] private IClientAuthenticationService? clientAuthenticationService { get; set; }

    [Inject] protected IAuthorizationService? AuthorizationService { get; set; }

    private bool isLoading => this.ListViewService.Records is null;

    private WeatherForecastsViewService ListViewService => this._listViewService!;
    private WeatherForecastViewService RecordViewService => this._recordViewService!;
    private WeatherForecastNotificationService NotificationService => this._notificationService!;

    public bool IsModal { get; set; }

    private IModalDialog? modalDialog { get; set; }

    private ComponentState loadState => isLoading ? ComponentState.Loading : ComponentState.Loaded;

    private UIVirtualizeListControl<DcoWeatherForecast>? uiVirtualizeListControl;

    protected override void OnInitialized()
    {
        this.NotificationService.RecordSetChanged += this.OnRecordSetChanged;
    }

    public async ValueTask<ItemsProviderResult<DcoWeatherForecast>> GetVirtualizedItems(ItemsProviderRequest request)
    {
        await this.ListViewService.GetForecastsAsync(request);
        if (this.ListViewService.Records is null)
            return new ItemsProviderResult<DcoWeatherForecast>(new List<DcoWeatherForecast>(), 0);

        return new ItemsProviderResult<DcoWeatherForecast>(this.ListViewService.Records, this.ListViewService.RecordCount);
    }


    private async Task DeleteRecord(Guid Id)
    {
        await this.RecordViewService.GetForecastAsync(Id);

        if (this.RecordViewService.Record is null)
        {
            toasterService?.AddToast(Toast.NewTTD("Delete", $"Delete Failed.  the record does not exist", MessageColour.Danger, 30));
            return;
        }

        if (!await this.CheckAuthorization(this.RecordViewService.Record, AppPolicies.IsEditor))
        {
            toasterService?.AddToast(Toast.NewTTD("Delete", $"Delete Failed.  You do not have permissions to delete this record", MessageColour.Danger, 30));
            return;
        }

        var transactionId = Guid.NewGuid();
        await this.RecordViewService.DeleteRecordAsync(transactionId, Id);

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
        var user = clientAuthenticationService?.GetCurrentIdentity();
        var transactionId = Guid.NewGuid();
        var record = new DcoWeatherForecast
        {
            Date = DateTime.Now,
            Id = Guid.NewGuid(),
            OwnerId = SessionTokenManagement.GetIdentityId(clientAuthenticationService?.GetCurrentIdentity()),
            Summary = "Balmy",
            TemperatureC = 14
        };
        await this.RecordViewService.AddRecordAsync(transactionId, record);

        var message = ResponseMessageStore?.GetMessage(transactionId);
        if (message is not null)
        {
            if (message.OK)
                toasterService?.AddToast(Toast.NewTTD("Add", $"Record added.", MessageColour.Success, 5));
            else
                toasterService?.AddToast(Toast.NewTTD("Add", $"Add Failed.  Server response: {message.Message}", MessageColour.Danger, 30));
        }
    }

    private void OnRecordSetChanged(object? sender, EventArgs e)
    { 
        this.uiVirtualizeListControl?.NotifyListChanged();
        this.InvokeAsync(this.StateHasChanged);
    }

    protected virtual async Task<bool> CheckAuthorization(DcoWeatherForecast record, string policy)
    {
        var state = await AuthTask!;
        var result = await this.AuthorizationService!.AuthorizeAsync(state.User, record, policy);
        return result.Succeeded;
    }

    public void Dispose()
        => this.NotificationService.RecordSetChanged -= this.OnRecordSetChanged;
}


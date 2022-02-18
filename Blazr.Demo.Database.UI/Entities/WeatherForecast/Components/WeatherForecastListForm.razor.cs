/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================
namespace Blazr.Demo.Database.UI;

public partial class WeatherForecastListForm : ComponentBase
{
    private PagingControl? pagingControl;
    private IModalDialog? modalDialog;
    private bool _firstLoad;

    [CascadingParameter] public Task<AuthenticationState>? AuthTask { get; set; }

    [Parameter] public Guid RouteId { get; set; } = Guid.Empty;

    [Inject] private WeatherForecastListService? _listViewService { get; set; }

    [Inject] private WeatherForecastCrudService? _recordViewService { get; set; }

    [Inject] private NavigationManager? _navigationManager { get; set; }

    [Inject] private ToasterService? _toasterService { get; set; }

    [Inject] private ResponseMessageStore? _responseMessageStore { get; set; }

    [Inject] private IClientAuthenticationService? _clientAuthenticationService { get; set; }

    [Inject] private IAuthorizationService? _authorizationService { get; set; }

    [Inject] private WeatherForecastNotificationService? _notificationService { get; set; }

    [Inject] private UiStateService? _uiStateService { get; set; }

    public bool IsModal { get; set; }

    private WeatherForecastListService ListViewService => this._listViewService!;
    private WeatherForecastCrudService RecordViewService => this._recordViewService!;
    private WeatherForecastNotificationService NotificationService => this._notificationService!;
    private NavigationManager NavigationManager => _navigationManager!;
    private ToasterService ToasterService => _toasterService!;
    private ResponseMessageStore ResponseMessageStore => _responseMessageStore!;
    private IClientAuthenticationService ClientAuthenticationService => _clientAuthenticationService!;
    private IAuthorizationService AuthorizationService => _authorizationService!;
    private UiStateService UiStateService => _uiStateService!;

    public ListContext listContext = new ListContext();
    
    private bool isLoading => ListViewService.Records is null;

    private ComponentState loadState => isLoading ? ComponentState.Loading : ComponentState.Loaded;

    protected override void OnInitialized()
    { 
        _firstLoad = true;
        this.listContext.Attach(_uiStateService, this.RouteId, this.GetPagedItems);
        this.NotificationService.RecordSetChanged += this.OnListChanged;
    }

    public async ValueTask<PagingOptions> GetPagedItems(PagingOptions request)
    {
        var listOptions = new ListOptions { PageSize = request.PageSize, StartRecord = request.StartRecord };
        var options = await this.ListViewService.GetForecastsAsync(this.GetState(listOptions));
        await this.InvokeAsync(StateHasChanged);
        this.SaveState(options);
        return new PagingOptions { PageSize = options.PageSize, StartRecord = options.StartRecord, ListCount = options.ListCount };
    }

    public async ValueTask<ListOptions> GetPagedItems(ListOptions request)
    {
        var options = await this.ListViewService.GetForecastsAsync(this.GetState(request));
        await this.InvokeAsync(StateHasChanged);
        this.SaveState(options);
        return options;
    }

    private void SaveState(ListOptions options)
    {
        if (this.RouteId != Guid.Empty)
            this.UiStateService.AddStateData(this.RouteId, options);
    }

    private ListOptions GetState(ListOptions options)
    {
        ListOptions? returnOptions = null;
        if (this.RouteId != Guid.Empty && this.UiStateService.TryGetStateData(this.RouteId, out object stateOptions) && stateOptions is ListOptions)
        {
            returnOptions = (stateOptions as ListOptions)!.Copy;
            if (!_firstLoad)
            {
                returnOptions.StartRecord = options.StartRecord;
                returnOptions.PageSize = options.PageSize;
            }
        }
        _firstLoad = false;
        returnOptions ??= options;
        return returnOptions;
    }

    private async Task DeleteRecord(Guid Id)
    {
        await this.RecordViewService.GetForecastAsync(Id);

        if (this.RecordViewService.Record is null)
        {
            this.ToasterService?.AddToast(Toast.NewTTD("Delete", $"Delete Failed.  the record does not exist", MessageColour.Danger, 9));
            return;
        }

        if (!await this.CheckAuthorization(this.RecordViewService.Record, AppPolicies.IsEditor))
        {
            this.ToasterService?.AddToast(Toast.NewTTD("Delete", $"Delete Failed.  You do not have permissions to delete this record", MessageColour.Danger, 9));
            return;
        }

        var transactionId = Guid.NewGuid();
        await this.RecordViewService.DeleteRecordAsync(transactionId, Id);

        var message = ResponseMessageStore?.GetMessage(transactionId);
        if (message is not null)
        {
            if (message.OK)
                this.ToasterService?.AddToast(Toast.NewTTD("Delete", $"Record Deleted.", MessageColour.Success, 3));
            else
                this.ToasterService?.AddToast(Toast.NewTTD("Delete", $"Delete Failed.  Server response: {message.Message}", MessageColour.Danger, 9));
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
            this.NavigationManager!.NavigateTo($"/WeatherForecast/Edit/{Id}");
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
            this.NavigationManager!.NavigateTo($"/WeatherForecast/View/{Id}");
    }

    private async Task AddRecordAsync()
    {
        var user = this.ClientAuthenticationService?.GetCurrentIdentity();
        var transactionId = Guid.NewGuid();
        var record = new DcoWeatherForecast
        {
            Date = DateTime.Now,
            WeatherForecastId = Guid.NewGuid(),
            OwnerId = SessionTokenManagement.GetIdentityId(this.ClientAuthenticationService?.GetCurrentIdentity()),
            Summary = "Balmy",
            TemperatureC = 14
        };
        await this.RecordViewService.AddRecordAsync(transactionId, record);

        var message = ResponseMessageStore?.GetMessage(transactionId);
        if (message is not null)
        {
            if (message.OK)
                this.ToasterService?.AddToast(Toast.NewTTD("Add", $"Record added.", MessageColour.Success, 3));
            else
                this.ToasterService?.AddToast(Toast.NewTTD("Add", $"Add Failed.  Server response: {message.Message}", MessageColour.Danger, 9));
        }
    }

    private void OnListChanged(object? sender, RecordSetChangedEventArgs e)
    {
        this.pagingControl?.NotifyListChanged();
        this.InvokeAsync(this.StateHasChanged);
    }

    protected virtual async Task<bool> CheckAuthorization(DcoWeatherForecast record, string policy)
    {
        var state = await AuthTask!;
        var result = await this.AuthorizationService!.AuthorizeAsync(state.User, new AppAuthFields { OwnerId = record.OwnerId }, policy);
        return result.Succeeded;
    }

    protected AppAuthFields GetAuthFields(DvoWeatherForecast record)
        => new AppAuthFields { OwnerId = record.OwnerId };

    public void Dispose()
        => this.NotificationService.RecordSetChanged -= this.OnListChanged;
}


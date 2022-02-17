/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================

namespace Blazr.UI.Bootstrap;

public partial class PagingControl<TRecord>
    : ComponentBase,
    IDisposable

    where TRecord : class, new()
{
    private ListOptions _listOptions = new ListOptions();
    private UiStateService UiStateService => _uiStateService!;
    private INotificationService NotificationService => _notificationService!;

    [Inject] private UiStateService? _uiStateService { get; set; }

    [Inject] private INotificationService? _notificationService { get; set; }

    [Parameter] public int PageSize { get; set; } = 5;

    [Parameter] public int BlockSize { get; set; } = 10;

    [Parameter] [EditorRequired] public ChangeListPage? PageList { get; set; }

    [Parameter] public Func<ListOptions, ListOptions>? Paging { get; set; }

    protected override void OnInitialized()
    {
        var restoreState = UiStateService.TryGetListOptionsData(typeof(TRecord), out ListOptions value);

        this.NotificationService.RecordSetPaged += this.OnPaged;

        _listOptions = restoreState
            ? value
            : new ListOptions { PageSize = this.PageSize };
        this.NotificationService.NotifyRecordSetPaging(this, new RecordSetPageEventArgs(_listOptions));

        UiStateService.AddListOptionsData(typeof(TRecord), _listOptions.Copy);
    }

    private void OnPaged(object? sender, RecordSetPageEventArgs e)
    {
        _listOptions.ListCount = e.Options.ListCount;
        this.Page = e.Options.Page;
        this.PageSize = e.Options.PageSize;
        this.ListCount = e.Options.ListCount;
    }

    private void GotToPage(int page)
    {
        if (page != this.Page)
        {
            this.Page = page;
            if (this.PageList is not null)
                this.PageList(this.ReadStartRecord, this.PageSize);
            this.InvokeAsync(this.StateHasChanged);
            UiStateService.AddListOptionsData(typeof(TRecord), _listOptions.Copy);
        }
    }

    private string GetCss(int page)
        => page == this.Page ? "btn-primary" : "btn-secondary";

    public void Dispose()
        => this.NotificationService.RecordSetPaged += this.OnPaged;

}

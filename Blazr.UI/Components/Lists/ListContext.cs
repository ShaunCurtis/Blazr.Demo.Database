/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================

namespace Blazr.UI;

public class ListContext
{
    private Guid _routeId = Guid.Empty;

    public SortOptions SortOptions { get; private set; } = new SortOptions();

    public ListOptions ListOptions { get; private set; } = new ListOptions();

    public PagingOptions PagingOptions { get; private set; } = new PagingOptions();
    
    private UiStateService? _uiStateService;

    public Func<ListOptions, ValueTask<ListOptions>>? ListProvider { get; set; }

    public bool HasStateService => _uiStateService != null; 

    public void Attach(UiStateService? uiStateService, Guid routeId, Func<ListOptions, ValueTask<ListOptions>>? listProvider)
    {
        _uiStateService = uiStateService;
        _routeId = routeId;
        this.ListProvider = listProvider;
        if (_uiStateService is not null)
            this.GetState();
    }

    public void SaveState()
    {
        if (_routeId != Guid.Empty && _uiStateService is not null)
            _uiStateService.AddStateData(_routeId, this.ListOptions);
    }

    public async ValueTask<PagingOptions> SetPagingState(PagingOptions pagingOptions)
    {
        this.ListOptions.StartRecord = pagingOptions.StartRecord;
        this.ListOptions.PageSize = pagingOptions.PageSize;
        if (ListProvider is not null)
        {
            var returnOptions = await ListProvider(this.ListOptions);
            if (returnOptions != null)
                this.ListOptions.ListCount = returnOptions.ListCount;
        }
        this.SaveState();
        return this.ListOptions.PagingOptions;
    }

    public void SetSortState(SortOptions sortOptions)
    {
        this.ListOptions.SortExpression = sortOptions.SortExpression;
        this.SaveState();
    }

    public ListOptions GetState()
    {
        if (_routeId != Guid.Empty && _uiStateService is not null && _uiStateService.TryGetStateData(_routeId, out object stateOptions) && stateOptions is ListOptions)
            this.ListOptions.Load(stateOptions as ListOptions);
        return this.ListOptions;
    }
}

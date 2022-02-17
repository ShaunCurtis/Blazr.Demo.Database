/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================

namespace Blazr.UI.Bootstrap;

public partial class PagingControl
    : ComponentBase
{
    private ListOptions _listOptions => new ListOptions() { PageSize = this.PageSize, Page = this.Page };
    private int Page = 0;
    private int ListCount = 0;

    [Parameter] public int PageSize { get; set; } = 5;

    [Parameter] public int BlockSize { get; set; } = 10;

    [Parameter] public Func<ListOptions, ValueTask<ListOptions>>? PagingProvider { get; set; }

    [Parameter] public bool ShowPageOf { get; set; } = true;

    protected async override Task OnInitializedAsync()
    {
        if (this.PagingProvider is not null)
        {
            var options = await PagingProvider(_listOptions);
            this.Page = options.Page;
            this.PageSize = options.PageSize;
            this.ListCount = options.ListCount;
        }
    }

    public async void NotifyListChanged()
        => await GotToPage();

    private int DisplayPage => this.Page + 1;

    private int LastPage => PageSize == 0 || ListCount == 0
        ? 0
        : ((int)Math.Ceiling(Decimal.Divide(this.ListCount, this.PageSize))) - 1;

    private int LastDisplayPage => this.LastPage + 1;

    private int ReadStartRecord => this.Page * this.PageSize;

    private int Block => (int)Math.Floor(Decimal.Divide(this.Page, this.BlockSize));

    private bool AreBlocks => this.ListCount > this.BlockSize * this.PageSize;

    private int BlockStartPage => this.Block * this.BlockSize;

    private int BlockEndPage => this.LastPage > (this.BlockStartPage + (BlockSize)) - 1
        ? (this.BlockStartPage + BlockSize) - 1
        : this.LastPage;

    private int LastBlock => (int)Math.Floor(Decimal.Divide(this.LastPage, this.BlockSize));

    private int LastBlockStartPage => LastBlock * this.BlockSize;

    private async Task GotToPage(int page)
    {
        if (this.PagingProvider is not null && page != this.Page)
        {
            this.Page = page;
            await GotToPage();
        }
    }

    private async Task GotToPage()
    {
        if (this.PagingProvider is not null)
        {
            var options = await PagingProvider(_listOptions);
            this.Page = options.Page;
            this.PageSize = options.PageSize;
            this.ListCount = options.ListCount;
            this.StateHasChanged();
        }
    }

    private string GetCss(int page)
        => page == this.Page ? "btn-primary" : "btn-secondary";

    private async Task MoveBlock(int block)
    {
        var _page = block switch
        {
            int.MaxValue => this.LastBlockStartPage,
            1 => this.Block + 1 > LastBlock ? LastBlock * this.BlockSize : this.BlockStartPage + BlockSize,
            -1 => this.Block - 1 < 0 ? 0 : this.BlockStartPage - BlockSize,
            _ => 0
        };
        await this.GotToPage(_page);
    }

    private async Task GoToBlock(int block)
        => await this.GotToPage(block * this.PageSize);
}

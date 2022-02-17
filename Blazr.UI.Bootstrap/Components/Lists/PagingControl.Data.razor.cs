/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================
namespace Blazr.UI.Bootstrap;

public partial class PagingControl<TRecord> : ComponentBase
    where TRecord : class, new()
{
    public int Page { get; private set; }

    public int ListCount { get; private set; }

    public int LastPage => PageSize == 0 || ListCount == 0
        ? 0
        : ((int)Math.Ceiling(Decimal.Divide(this.ListCount, this.PageSize))) - 1;

    public int LastDisplayPage => this.LastPage + 1;

    public int ReadStartRecord => this.Page * this.PageSize;

    public int Block => (int)Math.Floor(Decimal.Divide(this.Page, this.BlockSize));

    public bool AreBlocks => this.ListCount > this.BlockSize * this.PageSize;

    public int BlockStartPage => this.Block * this.BlockSize;

    public int BlockEndPage => this.LastPage > (this.BlockStartPage + (BlockSize)) -1
        ? (this.BlockStartPage + BlockSize) - 1
        : this.LastPage;

    public int LastBlock => (int)Math.Ceiling(Decimal.Divide(this.LastPage, this.BlockSize));

    public int LastBlockStartPage => (LastBlock-1) * this.BlockSize;

    private void MoveBlock(int block)
    {
        var _page = block switch
        {
            int.MaxValue => this.LastBlockStartPage,
            1 => this.Block + 1 > LastBlock ? LastBlock * this.BlockSize : this.BlockStartPage + BlockSize,
            -1 => this.Block - 1 < 0 ? 0 : this.BlockStartPage - BlockSize,
            _ => 0
        };
        this.GotToPage(_page);
    }

    private void GoToBlock(int block)
    {
        this.GotToPage(block * this.PageSize);
    }

}


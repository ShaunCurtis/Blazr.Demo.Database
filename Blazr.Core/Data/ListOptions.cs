/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================


namespace Blazr.Core;

public class ListOptions
{
    public string FilterExpression { get; set; } = string.Empty;

    public string SortExpression { get; set; } = String.Empty;

    public int PageSize { get; set; } = 1000;

    public int StartRecord { get; set; } = 0;

    public int ListCount { get; set; } = 0;

    public bool IsPaging => (PageSize > 0);

    public int Page
    {
        get =>  StartRecord / PageSize;
        set => StartRecord = value * PageSize;
    }

    public ListOptions GetCopy(int listcount)
    {
        var copy = this.Copy;
        copy.ListCount = listcount;
        return copy;
    }  

    public ListOptions Copy
        => new ListOptions()
        {
            ListCount = this.ListCount,
            FilterExpression = this.FilterExpression,
            SortExpression = this.SortExpression,
            PageSize = this.PageSize,
            StartRecord = this.StartRecord
        };
}


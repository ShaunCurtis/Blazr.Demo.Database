﻿/// ============================================================
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
        get => StartRecord / PageSize;
        set => StartRecord = value * PageSize;
    }

    public PagingOptions PagingOptions => new PagingOptions
    {
        PageSize = PageSize,
        StartRecord = StartRecord,
        ListCount = ListCount
    };

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

    public void Load(ListOptions? options)
    {
        if (options is null)
            return;

        this.ListCount = options.ListCount;
        this.FilterExpression = options.FilterExpression;
        this.SortExpression = options.SortExpression;
        this.PageSize = options.PageSize;
        this.StartRecord = options.StartRecord;
    }
}


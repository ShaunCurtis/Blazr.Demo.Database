/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================

namespace Blazr.Core;

public record SortOptions
{
    public string? SortField { get; init; }

    public bool Descending { get; init; }

    public bool IsSorting => !string.IsNullOrWhiteSpace(SortField);

    public string? SortExpression =>
        this.Descending  
        ? $"{this.SortField} desc" 
        : this.SortField;

    public static SortOptions GetSortOptions(ListOptions options)
    {
        if (options.SortExpression is null)
            return new SortOptions();

        var desc = false;
        var field = string.Empty;
        if (options.SortExpression.Contains("desc", StringComparison.CurrentCultureIgnoreCase))
            desc = true;

        var sortField = options.SortExpression.Replace("desc", "",StringComparison.CurrentCultureIgnoreCase);
        if (!string.IsNullOrWhiteSpace(sortField))
            field = sortField;

        return new SortOptions { Descending = desc, SortField=field  };
    }
}

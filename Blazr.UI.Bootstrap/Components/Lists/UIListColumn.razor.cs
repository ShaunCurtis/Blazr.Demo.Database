/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================

namespace Blazr.UI.Bootstrap;

public partial class UIListColumn : UIComponentBase
{
    private bool isMaxRowColumn => IsMaxColumn && !this.IsHeader;
    private bool isNormalRowColumn => !IsMaxColumn && !this.IsHeader;
    private bool _isSortField => !string.IsNullOrWhiteSpace(this.SortField);
    private bool _isCurrentSortField => this._listContext!.ListOptions.SortOptions.SortField?.Equals(this.SortField) ?? false;
    protected override List<string> UnwantedAttributes { get; set; } = new List<string>() { "class" };

    [CascadingParameter(Name = "IsHeader")] public bool IsHeader { get; set; }
 
    [CascadingParameter] private ListContext? _listContext { get; set; }
    
    [Parameter] public bool IsMaxColumn { get; set; }
    
    [Parameter] public string HeaderTitle { get; set; } = string.Empty;
    
    [Parameter] public bool IsHeaderNoWrap { get; set; }
    
    [Parameter] public bool NoWrap { get; set; }
    
    [Parameter] public string SortField { get; set; } = string.Empty;

    private void SortClick(MouseEventArgs e)
    {
        if (this._listContext is null)
            return;

        SortOptions options = _isCurrentSortField
            ?  new SortOptions { Descending = true, SortField = this._listContext.ListOptions.SortOptions.SortField }
            : new SortOptions { Descending = false, SortField = this.SortField };

        this._listContext?.SetSortState(options);
    }

    private string HeaderCss
        => CSSBuilder.Class()
            .AddClass(IsHeaderNoWrap, "header-column-nowrap", "header-column")
            .AddClass("text-nowrap", NoWrap)
            .AddClass("align-baseline")
            .Build();

    private string TDCss
        => CSSBuilder.Class()
            .AddClass(this.isMaxRowColumn, "max-column", "data-column")
            .AddClass("text-nowrap", this.NoWrap)
            .Build();

    private string SortIconCss
    => _listContext is null || !_isCurrentSortField
        ? UICssClasses.NotSortedClass
        : this._listContext.ListOptions.SortOptions.Descending
            ? UICssClasses.AscendingClass
            : UICssClasses.DescendingClass;

}


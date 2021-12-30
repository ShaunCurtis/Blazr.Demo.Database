/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================

using Blazr.Core.Toaster;

namespace Blazr.UI.Bootstrap;

public partial class Toaster : ComponentBase, IDisposable
{ 
    [Inject] private ToasterService? _toasterService { get; set; }

    private ToasterService toasterService => _toasterService!;

    protected override void OnInitialized()
        => this.toasterService.NewToast += OnNewToast;

    private void ClearToast(Toast toast)
        => toasterService.ClearToast(toast);

    private void OnNewToast(object? sender, NewToastEventArgs e)
        => this.InvokeAsync(this.StateHasChanged);

    public void Dispose()
        => this.toasterService.NewToast -= OnNewToast;
}


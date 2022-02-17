/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================

namespace Blazr.Core;

public interface INotificationService
{
    public event EventHandler<RecordSetChangedEventArgs>? RecordSetChanged;

    public event EventHandler<RecordChangedEventArgs>? RecordChanged;

    public event EventHandler<RecordSetPageEventArgs>? RecordSetPaged;

    public event EventHandler<RecordSetPageEventArgs>? RecordSetPaging;

    public void NotifyRecordSetChanged(object? sender, RecordSetChangedEventArgs e);

    public void NotifyRecordSetPaged(object? sender, RecordSetPageEventArgs e);

    public void NotifyRecordSetPaging(object? sender, RecordSetPageEventArgs e);

    public void NotifyRecordChanged(object? sender, RecordChangedEventArgs e);
}

/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================

namespace Blazr.Demo.Database.Core;

public class WeatherForecastNotificationService :
    INotificationService
{
    public event EventHandler<RecordSetChangedEventArgs>? RecordSetChanged;

    public event EventHandler<RecordChangedEventArgs>? RecordChanged;

    public event EventHandler<RecordSetPageEventArgs>? RecordSetPaging;

    public event EventHandler<RecordSetPageEventArgs>? RecordSetPaged;

    public void NotifyRecordSetChanged(object? sender, RecordSetChangedEventArgs e)
        => this.RecordSetChanged?.Invoke(this, e);

    public void NotifyRecordSetPaged(object? sender, RecordSetPageEventArgs e)
        => this.RecordSetPaged?.Invoke(this, e);

    public void NotifyRecordSetPaging(object? sender, RecordSetPageEventArgs e)
        => this.RecordSetPaging?.Invoke(this, e);

    public void NotifyRecordChanged(object? sender, RecordChangedEventArgs e)
        => this.RecordChanged?.Invoke(this, e);
}

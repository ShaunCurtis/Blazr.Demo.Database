namespace Blazr.Core;

public class RecordChangedEventArgs 
    : EventArgs
{
    public Guid RecordId { get; set; } = Guid.Empty;

    public static new RecordChangedEventArgs Empty => new RecordChangedEventArgs();

    public static RecordChangedEventArgs Create(Guid recordId)
        => new RecordChangedEventArgs { RecordId = recordId };
}

namespace Blazr.Core;

public class RecordSetPageEventArgs
    : EventArgs
{
    public ListOptions Options { get; set; } = new ListOptions();

    public RecordSetPageEventArgs() { }

    public RecordSetPageEventArgs(ListOptions options)
        => Options = options;

    public static new RecordSetPageEventArgs Empty => new RecordSetPageEventArgs();

    public static RecordSetPageEventArgs Create(ListOptions options)
        => new RecordSetPageEventArgs { Options = options };
}

# Building a Blazor Bootstrap Toaster

## Repo and Demo Site

You can find the code in my Blazr.Demo.Database site.  Search the repo for the relevant files.

- The Service files are in *Blazr.Core/Services/Toaster*.
- The Component is in *Blazr.UI.Bootstrap/Components/Toaster*.

A demo site can be found here at [https://blazr-demo.azurewebsites.net ](https://blazr-demo.azurewebsites.net)

### Toast

First `Toast`, declared as a `record`.  `TTD` - Time To Die - defines the expiration time for the Toast.

```csharp
public record Toast
{
    public Guid Id = Guid.NewGuid();
    public string Title { get; init; } = string.Empty;
    public string Message { get; init; } = string.Empty;
    public MessageColour MessageColour { get; init; } = MessageColour.Primary;
    public DateTimeOffset Posted = DateTimeOffset.Now;
    public DateTimeOffset TTD { get; init; } = DateTimeOffset.Now.AddSeconds(30);
    public bool IsBurnt => TTD < DateTimeOffset.Now;
    private TimeSpan elapsedTime => Posted - DateTimeOffset.Now;

    public string ElapsedTimeText =>
        elapsedTime.Seconds > 60
        ? $"posted {-elapsedTime.Minutes} mins ago"
        : $"posted {-elapsedTime.Seconds} secs ago";


    public static Toast NewTTD(string title, string message, MessageColour messageColour, int secsToLive)
        => new Toast
        {
            Title = title,
            Message = message,
            MessageColour = messageColour,
            TTD = DateTimeOffset.Now.AddSeconds(secsToLive)
        };
}
```

`MessageColour` is an `enum`, based on the fairly standard Css nomenclature.

```csharp
public enum MessageColour
{
    Primary, Secondary, Dark, Light, Success, Danger, Warning, Info
}
```

### Toaster Service

`ToasterService` is a DI service to hold and manage Toasts.  It has a private list to hold the toasts with add and clear methods.  There's a timer to trigger `ClearTTDs` to clear out expired toasts and if necessary raise the `ToasterChanged` event.  It also raises the `ToasterTimerElapsed` event on each cycle.

```csharp
public class ToasterService : IDisposable
{
    private readonly List<Toast> _toastList = new List<Toast>();
    private System.Timers.Timer _timer = new System.Timers.Timer();
    public event EventHandler? ToasterChanged;
    public event EventHandler? ToasterTimerElapsed;
    public bool HasToasts => _toastList.Count > 0;

    public ToasterService()
    {
        AddToast(new Toast { Title = "Welcome Toast", Message = "Welcome to this Application.  I'll disappear after 15 seconds.", TTD = DateTimeOffset.Now.AddSeconds(10) });
        _timer.Interval = 5000;
        _timer.AutoReset = true;
        _timer.Elapsed += this.TimerElapsed;
        _timer.Start();
    }

    public List<Toast> GetToasts()
    {
        ClearTTDs();
        return _toastList;
    }

    private void TimerElapsed(object? sender, ElapsedEventArgs e)
    { 
        this.ClearTTDs();
        this.ToasterTimerElapsed?.Invoke(this, EventArgs.Empty);
    }

    public void AddToast(Toast toast)
    {
        _toastList.Add(toast);
        // only raise the ToasterChanged event if it hasn't already been raised by ClearTTDs
        if (!this.ClearTTDs())
            this.ToasterChanged?.Invoke(this, EventArgs.Empty);
    }

    public void ClearToast(Toast toast)
    {
        if (_toastList.Contains(toast))
        {
            _toastList.Remove(toast);
            // only raise the ToasterChanged event if it hasn't already been raised by ClearTTDs
            if (!this.ClearTTDs())
                this.ToasterChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    private bool ClearTTDs()
    {
        var toastsToDelete = _toastList.Where(item => item.IsBurnt).ToList();
        if (toastsToDelete is not null && toastsToDelete.Count > 0)
        {
            toastsToDelete.ForEach(toast => _toastList.Remove(toast));
            this.ToasterChanged?.Invoke(this, EventArgs.Empty);
            return true;
        }
        return false;
    }

    public void Dispose()
    {
        if (_timer is not null)
        {
            _timer.Elapsed += this.TimerElapsed;
            _timer.Stop();
        }
    }
}
```

`ToasterService` can run as either a `Scoped` or `Singleton` service, depending on what you're using it for.

### Toaster

`Toaster` is the UI component to display toasts.

The razor markup implements the Bootstrap Toast markup, with a loop to add each toast.  The markup will display the toasts stacked in the top right.

```csharp
@implements IDisposable
@if (this.toasterService.HasToasts)
{
    <div class="">
        <div class="toast-container position-absolute top-0 end-0 mt-5 pt-5 pe-2">
            @foreach (var toast in this.toasterService.GetToasts())
            {
                var _toastCss = toastCss(toast);
                <div class="toast show" role="alert" aria-live="assertive" aria-atomic="true">
                    <div class="toast-header @_toastCss">
                        <strong class="me-auto">@toast.Title</strong>
                        <small class="@_toastCss">@toast.ElapsedTimeText</small>
                        <button type="button" class="btn-close btn-close-white" aria-label="Close" @onclick="() => this.ClearToast(toast)"></button>
                    </div>
                    <div class="toast-body">
                        @toast.Message
                    </div>
                </div>
            }
        </div>
    </div>
}
```

The code:

- injects the `ToasterService`
- Wires up event handlers to the two events.  These simply invoke `StateHasChanged` to re-render the component.  Note `StateHasChanged` is invoked on the UI thread: in Blazor Server the timer is probably running on another thread.
- Clears a toast if the X is clicked.
- Works out the correct CSS for the colour.

```csharp
public partial class Toaster : ComponentBase, IDisposable
{
    [Inject] private ToasterService? _toasterService { get; set; }

    private ToasterService toasterService => _toasterService!;

    protected override void OnInitialized()
    { 
        this.toasterService.ToasterChanged += ToastChanged;
        this.toasterService.ToasterTimerElapsed += ToastChanged;
    }

    private void ClearToast(Toast toast)
        => toasterService.ClearToast(toast);

    private void ToastChanged(object? sender, EventArgs e)
        => this.InvokeAsync(this.StateHasChanged);

    public void Dispose()
    { 
        this.toasterService.ToasterChanged -= ToastChanged;
        this.toasterService.ToasterTimerElapsed -= ToastChanged;
    }

    private string toastCss(Toast toast)
    {
        var colour = Enum.GetName(typeof(MessageColour), toast.MessageColour)?.ToLower();
        return toast.MessageColour switch
        {
            MessageColour.Light => "bg-light",
            _ => $"bg-{colour} text-white"
        };
    }
}
```

## Implementing

1. Add `ToasterService` to the DI service container in Program.

2. Add the component to either `Layout` or `App` or whereever you wish to use it.

```xml
<CascadingAuthenticationState>
    <Router AppAssembly="@typeof(App).Assembly" PreferExactMatches="@true">
    ....
    </Router>
</CascadingAuthenticationState>
<Toaster />
```

3. Inject the service into any pages you want to raise toasts, and call `AddToast`.

# Authentication Provider and the UI

The Blazor UI Authorization components use the DI registered `AuthenticationProvider` to access the currently authenticated entity.

The default `AuthenticationProvider` reads the User information encoded into the Http Header and creates a `ClaimsPrincipal` object from the information.  Authentication takes place "off SPA", so there's no in-built functionality to change entities.

To use onsite authentication with JWT tokens we need to rebuild the `AuthenticationProvider`.

#### ServiceAuthenticationStateProvider

`SimpleJwtAuthenticationStateProvider` inherits from `AuthenticationProvider`.  

1. It overrides `GetAuthenticationStateAsync` to get current identity from the `IClientAuthenticationService`.
2. It wires into the `OnAuthenticationChanged` event on `IClientAuthenticationService` and raises the inherited `AuthenticationStateChanged` on `AuthenticationProvider` by calling `NotifyAuthenticationStateChanged`.


```csharp
public class ServiceAuthenticationStateProvider : AuthenticationStateProvider, IDisposable
{
    private readonly IClientAuthenticationService _clientAuthenticationService;

    public ServiceAuthenticationStateProvider(IClientAuthenticationService clientAuthenticationService)
    {
        _clientAuthenticationService = clientAuthenticationService;
        _clientAuthenticationService.AuthenticationChanged += this.OnAuthenticationChanged;
    }

    public override Task<AuthenticationState> GetAuthenticationStateAsync()
        => Task.FromResult(new AuthenticationState(_clientAuthenticationService.GetCurrentIdentity()));

    private void OnAuthenticationChanged(object? sender, AuthenticationChangedEventArgs e)
    {
        if (e.Identity is not null)
            this.NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(e.Identity)));
    }

    public void Dispose()
        => _clientAuthenticationService.AuthenticationChanged -= this.OnAuthenticationChanged;

}
```

## UI

The UI is a simple component `UserBar` placed in the header bar.  It provides a simple dropdown to select the identity.

```html
<span class="mr-2">Change User:</span>
<div class="w-25">
    <select id="userselect" class="form-control" @onchange="ChangeUser">
        @foreach (var value in TestIdentities.IdentityList)
        {
            @if (value.Key == _currentUserId)
            {
                <option value="@value.Key" selected>@value.Value</option>
            }
            else
            {
                <option value="@value.Key">@value.Value</option>
            }
        }
    </select>
</div>
<span class="text-nowrap ms-3">
    <AuthorizeView>
        <Authorized>
            Hello, @this.user!.Identity!.Name
        </Authorized>
        <NotAuthorized>
            Not Logged In
        </NotAuthorized>
    </AuthorizeView>
</span>
```

1. It gets the current identity from the cascaded AuthTask.
2. It wires an event handler to the `AuthenticationStateChanged` event on `AuthenticationStateProvider`
3. It calls the `IClientAuthenticationService` to change the logged in Identity.
4. It gets the new user when an `AuthenticationStateChanged` event occurs.

The component triggers an authenticationState change by calling `LogInAsync` and then updates itself when the event has happened. 

```csharp
    [CascadingParameter] public Task<AuthenticationState>? AuthTask { get; set; }

    [Inject] private AuthenticationStateProvider? AuthState { get; set; }

    [Inject] private IClientAuthenticationService? ClientAuthenticationService { get; set; }

    private System.Security.Claims.ClaimsPrincipal user = new ClaimsPrincipal();
    private Guid _currentUserId = Guid.Empty;

    protected async override Task OnInitializedAsync()
    {
        var authState = await AuthTask!;
        this.user = authState.User;
        AuthState!.AuthenticationStateChanged += this.OnUserChanged;
    }

    private bool GetSelected(string value)
        => user.Identity!.Name!.Equals(value, StringComparison.CurrentCultureIgnoreCase);

    private async Task ChangeUser(ChangeEventArgs e)
    {
        if (e.Value is not null && Guid.TryParse(e.Value.ToString(), out Guid Id))
        {
            await ClientAuthenticationService!.LogInAsync(new IdentityLoginCredentials { Id = Id });
        }
    }

    private async void OnUserChanged(Task<AuthenticationState> state)
        => await this.GetUser(state);

    private async Task GetUser(Task<AuthenticationState> state)
    {
        var authState = await state;
        this.user = authState.User;
    }

    public void Dispose()
        => AuthState!.AuthenticationStateChanged -= this.OnUserChanged;
}
```
# Implementing Authorization

For most applications you should be implementing Policy based authorization.  It's a little more complex to set up, but the effort will pay in the longer term.

Polices can be based on any piece of data you have on an identity.  We'll look at custom policies further on, but for now we'll base policies on roles.  The `RecordEditor` policy applies to users with either `User` or `Admin` role.  If you need to add a new role called `Editor`, you only need add the `Editor` role to the `RecordEditor` policy.  If you used Roles based authorization, you would need to go through your application and update all the authorize components.

The best method to build the application policies is to use a static class and the `AuthorizationPolicyBuilder` factory class.  Here's the Demo application's `AppPolicies` class.  Note:

1. We define constant strings to use throughout the application.
2. Each Policy is declared separately.
3. There's a Dictionary of all the policies for the application. 

```csharp
public static class AppPolicies
{
    public const string IsAdmin = "IsAdmin";
    public const string IsUser = "IsUser";
    public const string IsVisitor = "IsVisitor";

    public static AuthorizationPolicy IsAdminPolicy
        => new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .RequireRole("Admin")
        .Build();

    public static AuthorizationPolicy IsUserPolicy
        => new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .RequireRole("Admin", "User")
        .Build();

    public static AuthorizationPolicy IsVisitorPolicy
        => new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .RequireRole("Admin", "User", "Visitor")
        .Build();

    public static Dictionary<string, AuthorizationPolicy> Policies
    {
        get
        {
            var policies = new Dictionary<string, AuthorizationPolicy>();
            policies.Add(IsAdmin, IsAdminPolicy);
            policies.Add(IsUser, IsUserPolicy);
            policies.Add(IsVisitor, IsVisitorPolicy);
            return policies;
        }
    }
}
```

We can now apply these policies to our application like this:

```
services.AddAuthorization(config =>
{
    foreach (var policy in AppPolicies.Policies)
    {
        config.AddPolicy(policy.Key, policy.Value);
    }
});
```

## Custom UI Components

While the `AuthorizeView` components fit most situations, the markup can get a little verbose.  Buttons in a button bar are a good example.  It's fairly easy to build authorization into a button control.

The Button component class below accepts a `Policy` parameter.  It gets the current user from the cascaded `AuthenticationState` task and uses the `IAuthorizationService.AuthorizeAsync` to authorize the user against the policy.

```
public class UIAuthorizeButton : UIComponent
{
    [Parameter] public string Policy { get; set; } = String.Empty;
    
    [CascadingParameter] public Task<AuthenticationState>? AuthTask { get; set; }

    [Inject] private IAuthorizationService? authorizationService { get; set; }

    public UIAuthorizeButton()
        => this.CssClasses.Add("btn me-1");

    protected override void OnInitialized()
    {
        if (AuthTask is null)
            throw new Exception($"{this.GetType().FullName} must have access to cascading Paramater {nameof(AuthTask)}");
    }

    protected override string HtmlTag => "button";

    protected override async void BuildRenderTree(RenderTreeBuilder builder)
    {
        if (this.Show && await this.CheckPolicy())
        {
            builder.OpenElement(0, this.HtmlTag);
            builder.AddAttribute(1, "class", this.CssClass);
            builder.AddMultipleAttributes(2, this.SplatterAttributes);

            if (!UserAttributes.ContainsKey("type"))
                builder.AddAttribute(3, "type", "button");

            if (Disabled)
                builder.AddAttribute(4, "disabled");

            if (ClickEvent.HasDelegate)
                builder.AddAttribute(5, "onclick", EventCallback.Factory.Create<MouseEventArgs>(this, ClickEvent));

            builder.AddContent(6, ChildContent);
            builder.CloseElement();
        }
    }

    protected async Task<bool> CheckPolicy()
    {
        var state = await AuthTask!;
        var result = await this.authorizationService!.AuthorizeAsync(state.User, null, Policy);
        return result.Succeeded;
    }
}

```

The markup for the button looks like this:

```csharp
<UIAuthorizeButton Policy=@AppPolicies.IsUser class="btn-dark" ClickEvent="AddRecordAsync">Add Random Record</UIAuthorizeButton>
```

## Some Key points:

This confuses a lot of people:

```
[CascadingParameter] public Task<AuthenticationState>? AuthTask { get; set; }
```

Why do we have to call a Task to get this information?  You don't.  99% of the time the Task will be completed, and

```
  var state = await AuthTask;
``` 

simply assigns the `AuthenticationState` to `state`.  So why use a Task?  To cover the 1% when `AuthenticationState` is in transition: Authentication is a async process. 
/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================

using Blazr.Auth.JWT.Simple;
using Blazr.Demo.Database;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<Blazr.Demo.Database.UI.App>("#app");
//builder.RootComponents.Add<Blazr.UI.Bootstrap.TestApp>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
var services = builder.Services;
{
    services.AddSimpleJwtWASMAuthentication();
    services.AddAppBlazorWASMServices();
    services.AddAuthorizationCore(config =>
    {
        config.AddPolicy(AppPolicies.IsAdmin, AppPolicies.IsAdminPolicy);
        config.AddPolicy(AppPolicies.IsUser, AppPolicies.IsUserPolicy);
        config.AddPolicy(AppPolicies.IsVisitor, AppPolicies.IsVisitorPolicy);
    });

    services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
}

await builder.Build().RunAsync();

/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================

using Blazr.Auth.JWT.Simple;
using Blazr.Auth.JWT.Simple.Core;
using Blazr.Demo.Database;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var services = builder.Services;
{
    services.AddRazorPages();
    services.AddServerSideBlazor();
    services.AddAppBlazorServerServices();
    services.AddSimpleJwtServerAuthentication();
    services.Configure<JwtTokenSetup>(builder.Configuration.GetSection("JwtTokenSetup"));
    services.AddAuthorization(config =>
    {
        config.AddPolicy(AppPolicies.IsAdmin, AppPolicies.IsAdminPolicy);
        config.AddPolicy(AppPolicies.IsUser, AppPolicies.IsUserPolicy);
        config.AddPolicy(AppPolicies.IsVisitor, AppPolicies.IsVisitorPolicy);
    });

}

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();

using Blazr.Auth;
using Blazr.Demo.Database;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var services = builder.Services;
{
    services.AddControllersWithViews();
    services.AddRazorPages();

    services.AddAppWASMServerServices();
    services.AddControllers().PartManager.ApplicationParts.Add(new AssemblyPart(typeof(Blazr.Demo.Database.Controllers.WeatherForecastController).Assembly));
    services.AddControllers().PartManager.ApplicationParts.Add(new AssemblyPart(typeof(Blazr.Auth.Controllers.JwtAuthenticationController).Assembly));
    services.Configure<JwtTokenSetup>(builder.Configuration.GetSection("JwtTokenSetup"));
    services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidIssuer = builder.Configuration.GetSection("JwtTokenSetup").GetValue<string>("Issuer"),
                ValidateIssuer = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("JwtTokenSetup").GetValue<string>("Key"))),
                ValidateIssuerSigningKey = true,
                RequireExpirationTime = true,
                LifetimeValidator = SessionTokenManagement.JwtTokenLifetimeValidator,
            };
        });
    services.AddWASMServerSimpleJwtAuthentication();
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

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();


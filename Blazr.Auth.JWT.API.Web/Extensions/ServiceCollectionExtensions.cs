/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================
namespace Blazr.Auth.JWT.API.Web;

public static class ServiceCollectionExtensions
{
    public static void AddSimpleJwtServerAuthentication(this WebApplicationBuilder builder)
    {
        var services = builder.Services;
        services.AddSingleton<SimpleIdentityStore>();
        services.AddSingleton<IIdentityDataBroker, SimpleIdentityDataBroker>();
        services.AddSingleton<IJwtAuthenticationIssuer, SimpleJwtServerAuthenticationIssuer>();
        services.AddScoped<IClientAuthenticationService, SimpleJwtServerClientAuthenticationService>();
        services.AddScoped<AuthenticationStateProvider, ServiceAuthenticationStateProvider>();
        services.Configure<JwtTokenSetup>(builder.Configuration.GetSection("JwtTokenSetup"));
        services.AddAuthorization(config =>
        {
            config.AddPolicy(AppPolicies.IsAdmin, AppPolicies.IsAdminPolicy);
            config.AddPolicy(AppPolicies.IsUser, AppPolicies.IsUserPolicy);
            config.AddPolicy(AppPolicies.IsVisitor, AppPolicies.IsVisitorPolicy);
        });
    }

    public static void AddSimpleJwtWASMServerAuthentication(this WebApplicationBuilder builder)
    {
        var services = builder.Services;
        services.AddSingleton<SimpleIdentityStore>();
        services.AddSingleton<IIdentityDataBroker, SimpleIdentityDataBroker>();
        services.AddSingleton<IJwtAuthenticationIssuer, SimpleJwtServerAuthenticationIssuer>();
        services.AddControllers().PartManager.ApplicationParts.Add(new AssemblyPart(typeof(Blazr.Auth.JWT.API.Web.JwtAuthenticationController).Assembly));
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

        services.AddAuthorization(config =>
        {
            config.AddPolicy(AppPolicies.IsAdmin, AppPolicies.IsAdminPolicy);
            config.AddPolicy(AppPolicies.IsUser, AppPolicies.IsUserPolicy);
            config.AddPolicy(AppPolicies.IsVisitor, AppPolicies.IsVisitorPolicy);
        });
    }
}


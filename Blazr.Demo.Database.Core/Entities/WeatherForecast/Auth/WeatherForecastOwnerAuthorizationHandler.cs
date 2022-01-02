
using Blazr.Auth.JWT.Simple;
/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================
namespace Blazr.Demo.Database.Core;

public class WeatherForecastEditorAuthorizationRequirement : IAuthorizationRequirement { }

public class WeatherForecastOwnerAuthorizationHandler : AuthorizationHandler<WeatherForecastEditorAuthorizationRequirement, DcoWeatherForecast>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, WeatherForecastEditorAuthorizationRequirement requirement, DcoWeatherForecast resource)
    {
        var entityId = SessionTokenManagement.GetIdentityId(context.User);
        if (entityId != Guid.Empty && entityId == resource.OwnerId)
            context.Succeed(requirement);

        return Task.CompletedTask;
    }
}

public class WeatherForecastEditorAuthorizationHandler : AuthorizationHandler<WeatherForecastEditorAuthorizationRequirement, DcoWeatherForecast>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, WeatherForecastEditorAuthorizationRequirement requirement, DcoWeatherForecast resource)
    {
        if (context.User.IsInRole(SimpleJWTPolicies.Admin))
            context.Succeed(requirement);

        return Task.CompletedTask;
    }
}

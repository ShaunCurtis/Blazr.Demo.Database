/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================

using Blazr.Auth.JWT.Simple;

namespace Blazr.Demo.Database.Core;

public class RecordEditorAuthorizationRequirement : IAuthorizationRequirement { }

public class RecordOwnerAuthorizationHandler : AuthorizationHandler<RecordEditorAuthorizationRequirement, object>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, RecordEditorAuthorizationRequirement requirement, object data)
    {
        var entityId = SessionTokenManagement.GetIdentityId(context.User);
        if (data is not null && data is AppAuthFields)
        {
            var appFields = (data as AppAuthFields)!;
            if (entityId != Guid.Empty && entityId == appFields.OwnerId)
                context.Succeed(requirement);
        }
        return Task.CompletedTask;
    }
}

public class RecordEditorAuthorizationHandler : AuthorizationHandler<RecordEditorAuthorizationRequirement, object>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, RecordEditorAuthorizationRequirement requirement, object data)
    {
        if (context.User.IsInRole(SimpleJWTPolicies.Admin))
            context.Succeed(requirement);

        return Task.CompletedTask;
    }
}

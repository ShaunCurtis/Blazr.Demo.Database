/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================

using Blazr.Auth.Core;

namespace Blazr.Auth.Data;

public class SimpleIdentityStore
{
    public Task<ClaimsPrincipal> GetIdentityAsync(IdentityLoginCredentials userCredentials)
        => Task.FromResult(TestIdentities.IdentityIdCollection[userCredentials.Id] ??  new ClaimsPrincipal());
}


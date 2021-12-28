/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================

using Blazr.Auth.JWT.Simple.Core;

namespace Blazr.Auth.JWT.Simple.Data;

public class SimpleIdentityStore
{
    public Task<ClaimsPrincipal?> GetIdentityAsync(IdentityLoginCredentials userCredentials)
    {
        TestIdentities.IdentityIdCollection.TryGetValue(userCredentials.Id, out ClaimsPrincipal? principal);
        return Task.FromResult(principal);
    }
}


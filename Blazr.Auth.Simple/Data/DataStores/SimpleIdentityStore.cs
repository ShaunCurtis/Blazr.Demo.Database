/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================

using Blazr.Auth.Core;

namespace Blazr.Auth.Data;

public class SimpleIdentityStore
{
    public bool TryGetIdentity(IdentityLoginCredentials userCredentials, out ClaimsPrincipal identity)
    {
        var result = TestIdentities.IdentityIdCollection[userCredentials.Id];
        identity = result ?? new ClaimsPrincipal();
        return result is not null;
    }
}


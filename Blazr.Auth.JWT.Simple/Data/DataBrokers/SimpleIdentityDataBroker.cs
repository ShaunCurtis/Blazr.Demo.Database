/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================

using Blazr.Auth.JWT.Simple.Core;

namespace Blazr.Auth.JWT.Simple.Data;

public class SimpleIdentityDataBroker : IIdentityDataBroker
{
    private readonly SimpleIdentityStore _simpleIdentityStore;

    public SimpleIdentityDataBroker(SimpleIdentityStore simpleIdentityStore)
        => _simpleIdentityStore = simpleIdentityStore;

    public Task<ClaimsPrincipal?> GetIdentityAsync(IdentityLoginCredentials userCredentials)
        => _simpleIdentityStore.GetIdentityAsync(userCredentials);
}


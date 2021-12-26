/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================

namespace Blazr.Auth.Core;

public interface IIdentityDataBroker
{
    public Task<ClaimsPrincipal> GetIdentityAsync(IdentityLoginCredentials userCredentials);
}


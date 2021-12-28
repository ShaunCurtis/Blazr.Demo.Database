/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================

namespace Blazr.Auth.JWT.Simple.Core;

/// <summary>
/// class defining the fields required by the authenticator to log in an identity
/// </summary>    
public class IdentityLoginCredentials
{
    public Guid Id { get; set; }

    public string UserName { get; set; } = String.Empty;
}


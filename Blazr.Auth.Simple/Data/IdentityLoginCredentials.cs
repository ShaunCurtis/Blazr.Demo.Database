/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================

namespace Blazr.Auth
{
    /// <summary>
    /// class defining the fields required by the authenticator to log in an identity
    /// </summary>    
    public class IdentityLoginCredentials
    {
        public string UserName { get; set; } = String.Empty;
    }
}

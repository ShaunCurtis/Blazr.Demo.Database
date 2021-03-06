/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================


using Blazr.Auth.JWT.Simple.Core;

public class AuthenticationChangedEventArgs : EventArgs
{
    public ClaimsPrincipal? Identity { get; set; } = null;

    public static AuthenticationChangedEventArgs NewAuthenticationChangedEventArgs(ClaimsPrincipal identity)
        => new AuthenticationChangedEventArgs() { Identity = identity };
}


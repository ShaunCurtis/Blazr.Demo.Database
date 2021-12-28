/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================

namespace Blazr.Auth.JWT.Simple.Core;

public class JwtTokenSetup
{
    public string Issuer { get; set; } = String.Empty;

    public string Audience { get; set; } = String.Empty;

    public string Key { get; set; } = String.Empty;

    public int ExpireSeconds { get; set; }

    public int TokenStoreExpireMinutes { get; set; } = 60;

}


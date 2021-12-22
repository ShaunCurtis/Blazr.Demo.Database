/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================

namespace Blazr.Auth
{
    public class JwtTokenSetup
    {
        public string Issuer { get; set; }

        public string Audience { get; set; }

        public string Key { get; set; }

        public int ExpireSeconds { get; set; }

        public int TokenStoreExpireMinutes { get; set; } = 60;

    }
}

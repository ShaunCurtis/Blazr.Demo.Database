/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================


namespace Blazr.Auth.Core;

/// <summary>
/// Class that holds the client seesion information. 
/// Passed to the client on successful authentication.
/// </summary>
public record SessionToken
{
    public Guid SessionId { get; init; } = Guid.NewGuid();

    public string JwtToken { get; init; } = String.Empty;

    public bool IsAuthenticated { get; init; }

    public bool IsEmpty => string.IsNullOrWhiteSpace(this.JwtToken);
}


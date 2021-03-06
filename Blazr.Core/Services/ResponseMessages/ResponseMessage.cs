/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================

namespace Blazr.Core;

public record ResponseMessage
{
    public Guid Id { get; init; } = Guid.NewGuid();

    public string Message { get; init; } = String.Empty;

    public bool OK { get; init; } = true;

    public DateTimeOffset TTD { get; init; } = DateTimeOffset.Now.AddSeconds(15);

    public static ResponseMessage NewResponseMessage (Guid id, string message, bool oK, int secsToLive )
        => new ResponseMessage { Id = id, Message = message, OK = oK, TTD = DateTimeOffset.Now.AddSeconds(secsToLive) };
}

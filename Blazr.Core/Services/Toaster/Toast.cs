/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================

namespace Blazr.Core.Toaster;

public record Toast
{
    public Guid Id  = Guid.NewGuid();

    public string Title { get; init; } = string.Empty;

    public string Message { get; init; } = string.Empty;

    public DateTimeOffset Posted = DateTimeOffset.Now;

    public DateTimeOffset TTD { get; init; } = DateTimeOffset.Now.AddMinutes(2);

    private TimeSpan elapsedTime  => Posted - DateTimeOffset.Now;

    public string ElapsedTimeText =>
        elapsedTime.Seconds > 60
        ? $"{-elapsedTime.Minutes} mins ago"
        : $"{-elapsedTime.Seconds} secs ago";
}


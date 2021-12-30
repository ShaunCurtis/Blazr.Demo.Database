/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================


namespace Blazr.Core.Toaster;

    public class NewToastEventArgs : EventArgs
    {
        public Toast Toast { get; set; } = new Toast();
    }


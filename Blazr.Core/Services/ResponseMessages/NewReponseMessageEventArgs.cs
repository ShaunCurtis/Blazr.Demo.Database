/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================


namespace Blazr.Core;

    public class ReponseMessageEventArgs : EventArgs
    {
        public Guid Id { get; set; } = Guid.Empty;

        public ResponseMessage Message { get; set; } = new ResponseMessage();
    }


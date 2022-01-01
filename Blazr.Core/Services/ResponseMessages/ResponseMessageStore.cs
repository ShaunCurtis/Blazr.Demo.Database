/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================


namespace Blazr.Core;

public class ResponseMessageStore
{

    private readonly List<ResponseMessage> _responseMessages = new List<ResponseMessage>();

    public event EventHandler<ReponseMessageEventArgs>? NewResponseMessage;

    public ResponseMessage? GetMessage(Guid Id)
        => _responseMessages.FirstOrDefault(x => x.Id == Id);

    public bool TryGetMessage(Guid Id, out ResponseMessage message)
    {
        ClearTTDs();
        var ok = _responseMessages.Any(x => x.Id == Id);
        message = new ResponseMessage();
        if (ok)
            message = _responseMessages.FirstOrDefault(x => x.Id == Id) ?? new ResponseMessage();
        return ok;
    }

    public void ClearMessage(ResponseMessage message)
    {
        if (_responseMessages.Contains(message))
            _responseMessages.Remove(message);
        ClearTTDs();
    }

    public void AddMessage(ResponseMessage message)
    {
        if (!_responseMessages.Contains(message))
            _responseMessages.Add(message);
        ClearTTDs();
        this.NewResponseMessage?.Invoke(this, new ReponseMessageEventArgs { Id = message.Id, Message = message });
    }

    private void ClearTTDs()
    {
        var messagesToDelete = _responseMessages.Where(item => item.TTD < DateTimeOffset.Now).ToList();
        messagesToDelete.ForEach(message => _responseMessages.Remove(message));
    }
}


/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================

namespace Blazr.Core.Toaster;

    public class ToasterService
    {
        private readonly List<Toast> _toastList = new List<Toast>();

    public event EventHandler<NewToastEventArgs>? NewToast;

    public bool HasToasts => _toastList.Count > 0;  
    
    public ToasterService()
    {
        AddToast(new Toast { Title = "Test Toast", Message = "Test Message" });
        AddToast(new Toast { Title = "Test Toast 2", Message = "Test Message 2" });
    }

    public List<Toast> GetToasts()
    {
        ClearTTDs();
        return _toastList;
    }

    public void AddToast(Toast toast)
        => _toastList.Add(toast);

    public void ClearToast(Toast toast)
    => _toastList.Remove(toast);

    private void ClearTTDs()
    {
        var toastsToDelete = _toastList.Where(item => item.TTD < DateTimeOffset.Now).ToList();
        toastsToDelete.ForEach(toast => _toastList.Remove(toast));
    }

}


/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================

namespace Blazr.UI;
public class UiStateService
{
    private Dictionary<Guid, object> _stateItems = new Dictionary<Guid, object>();

    public void AddStateData(Guid Id, object value) 
    {
        if (_stateItems.ContainsKey(Id))
            _stateItems[Id] = value;
        else
            _stateItems.Add(Id, value);
    }

    public void ClearStateDataData(Guid Id)
    {
        if (_stateItems.ContainsKey(Id))
            _stateItems.Remove(Id);
    }

    public bool TryGetStateData(Guid Id, out object value)
    {
        var isdata = _stateItems.ContainsKey(Id);
        value = isdata
            ? _stateItems[Id]
            : new ListOptions();
        return isdata;
    }
}


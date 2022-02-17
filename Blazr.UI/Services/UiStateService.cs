/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================

namespace Blazr.UI;
public class UiStateService
{
    private Dictionary<Type, ListOptions> _listOptions = new Dictionary<Type, ListOptions>();

    public void AddListOptionsData(Type type, ListOptions value)
    {
        if (_listOptions.ContainsKey(type))
            _listOptions[type] = value;
        else
            _listOptions.Add(type, value);
    }

    public void ClearListOptionsData(Type type)
    {
        if (_listOptions.ContainsKey(type))
            _listOptions.Remove(type);
    }

    public bool TryGetListOptionsData(Type type, out ListOptions value)
    {
        var isdata = _listOptions.ContainsKey(type);
        value = isdata
            ? _listOptions[type]
            : new ListOptions();
        return isdata;
    }
}


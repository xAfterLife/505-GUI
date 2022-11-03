using System;

namespace _505_GUI_Battleships.MVVM.Model;

public sealed class ChangeViewModel
{
    public static event EventHandler<object>? ViewChanged;

    public static void ChangeView(Type t)
    {
        var view = Activator.CreateInstance(t) ?? null;
        if ( view == null ) return;
            OnViewChanged(view);
    }

    private static void OnViewChanged(object e)
    {
        ViewChanged?.Invoke(null, e);
    }
}

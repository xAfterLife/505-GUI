using System;
using System.Collections.Generic;
using _505_GUI_Battleships.MVVM.ViewModel;

namespace _505_GUI_Battleships.Core;

public sealed class ChangeViewModel
{
    public static event EventHandler<object>? ViewChanged;

    public enum ViewType
    {
        Dummy = 0,
        Start = 1
    }

    private static readonly Dictionary<ViewType, Type> Views = new()
    {
        { ViewType.Dummy, typeof(DummyViewModel) },
        { ViewType.Start, typeof(StartViewModel) }
    };

    public static void ChangeView(ViewType viewType)
    {
        var view = Activator.CreateInstance(Views[viewType]);
        if ( view == null ) return;
            OnViewChanged(view);
    }

    private static void OnViewChanged(object e)
    {
        ViewChanged?.Invoke(null, e);
    }
}

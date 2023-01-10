using System;
using System.Collections.Generic;
using _505_GUI_Battleships.MVVM.ViewModel;

namespace _505_GUI_Battleships.Core;

public sealed class ChangeViewModel
{
    /// <summary>
    ///     Type of the ViewModel
    ///     (f.E. Dummy/Start/PlayerSelection/etc)
    /// </summary>
    public enum ViewType
    {
        Start = 0,
        PlayerSelection = 1,
        GameOptions = 2,
        ShipSelection = 3,
        SelectTargetPlayer = 4
    }

    /// <summary>
    ///     Collection of our Views as Enum + Type Couples
    /// </summary>
    private static readonly Dictionary<ViewType, Type> Views = new()
    {
        { ViewType.Start, typeof(StartViewModel) },
        { ViewType.PlayerSelection, typeof(PlayerSelectionViewModel) },
        { ViewType.ShipSelection, typeof(ShipSelectionViewModel) },
        { ViewType.GameOptions, typeof(GameOptionsViewModel) },
        { ViewType.SelectTargetPlayer, typeof(GameOptionsViewModel) }
    };

    /// <summary>
    ///     Event for MainViewModel to ChangeView
    /// </summary>
    public static event EventHandler<object>? ViewChanged;

    /// <summary>
    ///     Creates a View based on the given Enum and passes it to the Event Invoker
    /// </summary>
    /// <param name="viewType">Enum ViewType to guarantee safety of passed typed</param>
    public static void ChangeView(ViewType viewType)
    {
        var view = Activator.CreateInstance(Views[viewType]);
        if ( view == null )
            return;
        OnViewChanged(view);
    }

    /// <summary>
    ///     Raises the OnViewChanged event
    /// </summary>
    /// <param name="e">View as EventArgs</param>
    private static void OnViewChanged(object e)
    {
        ViewChanged?.Invoke(null, e);
    }
}

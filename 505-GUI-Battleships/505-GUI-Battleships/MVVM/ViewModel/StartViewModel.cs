using System.Windows;
using System.Windows.Input;
using _505_GUI_Battleships.Core;

namespace _505_GUI_Battleships.MVVM.ViewModel;

internal class StartViewModel : ObservableObject
{
    /// <summary>
    ///     Command to Change the CurrentView to the PlayerSelection
    /// </summary>
    public static ICommand StartGameCommand => new RelayCommand(_ => ChangeViewModel.ChangeView(ChangeViewModel.ViewType.PlayerSelection));

    public static ICommand StartShipSelectionCommand => new RelayCommand(_ => ChangeViewModel.ChangeView(ChangeViewModel.ViewType.ShipSelection));

    /// <summary>
    ///     Command to Exit the Current Application
    /// </summary>
    public static ICommand ExitCommand => new RelayCommand(_ => Application.Current.Shutdown());
}

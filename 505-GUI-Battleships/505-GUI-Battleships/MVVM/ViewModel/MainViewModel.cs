using System.Windows;
using System.Windows.Input;
using _505_GUI_Battleships.Core;

namespace _505_GUI_Battleships.MVVM.ViewModel;

public class MainViewModel : ObservableObject
{
    private object _currentView = new StartViewModel();

    public object CurrentView
    {
        get => _currentView;
        set => Update(ref _currentView, value);
    }

    public static ICommand ExitCommand => new RelayCommand(_ => Application.Current.Shutdown());
    public static ICommand MaximizeCommand => new RelayCommand(_ => WState = WState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized);
    public static ICommand MinimizeCommand => new RelayCommand(_ => WState = WState == WindowState.Minimized ? WindowState.Normal : WindowState.Minimized);

    internal static WindowState WState
    {
        get => Application.Current.MainWindow!.WindowState;
        set => Application.Current.MainWindow!.WindowState = value;
    }

    public MainViewModel()
    {
        ChangeViewModel.ViewChanged += (_, view) =>
        {
            CurrentView = view;
        };
    }
}

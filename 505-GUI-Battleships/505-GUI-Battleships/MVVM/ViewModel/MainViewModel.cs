using System.Windows;
using System.Windows.Input;
using _505_GUI_Battleships.Core;

namespace _505_GUI_Battleships.MVVM.ViewModel;

public class MainViewModel : ObservableObject
{
    /// <summary>
    ///     Backingfield for the CurrentView
    /// </summary>
    private object _currentView = new StartViewModel();

    /// <summary>
    ///     CurrentView to generate View
    /// </summary>
    public object CurrentView
    {
        get => _currentView;
        set => Update(ref _currentView, value);
    }

    /// <summary>
    ///     Command to Exit the Application
    /// </summary>
    public static ICommand ExitCommand => new RelayCommand(_ => Application.Current.Shutdown());

    /// <summary>
    ///     Command to Maximize/Normalize the Application
    /// </summary>
    public static ICommand MaximizeCommand => new RelayCommand(_ => WState = WState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized);

    /// <summary>
    ///     Command to Minimize/Normalize the Application
    /// </summary>
    public static ICommand MinimizeCommand => new RelayCommand(_ => WState = WState == WindowState.Minimized ? WindowState.Normal : WindowState.Minimized);

    /// <summary>
    ///     WindowState wrapper
    /// </summary>
    internal static WindowState WState
    {
        get => Application.Current.MainWindow!.WindowState;
        set => Application.Current.MainWindow!.WindowState = value;
    }

    /// <summary>
    ///     ctor
    /// </summary>
    public MainViewModel()
    {
        //Subscribe to ViewChanged and set the CurrentView value
        ChangeViewModel.ViewChanged += (_, view) =>
        {
            CurrentView = view;
        };
    }
}

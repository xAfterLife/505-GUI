using System.Windows;
using _505_GUI_Battleships.Core;

namespace _505_GUI_Battleships.MVVM.ViewModel;

internal class MainViewModel : ObservableObject
{
    internal readonly DummyViewModel? DummyVm;
    internal readonly StartViewModel? StartVm;

    private object _currentView;

    public RelayCommand StartViewModelCommand { get; set; }
    public RelayCommand DummyViewModelCommand { get; set; }
    public RelayCommand ExitCommand { get; set; }
    public RelayCommand MaximizeCommand { get; set; }
    public RelayCommand MinimizeCommand { get; set; }

    public object CurrentView
    {
        get => _currentView;
        set
        {
            _currentView = value;
            OnPropertyChanged();
        }
    }

    internal static WindowState WState
    {
        get => Application.Current.MainWindow!.WindowState;
        set => Application.Current.MainWindow!.WindowState = value;
    }

    public MainViewModel()
    {
        StartVm = new StartViewModel();
        DummyVm = new DummyViewModel();

        _currentView = StartVm;

        ExitCommand = new RelayCommand(_ =>
        {
            Application.Current.Shutdown();
        });

        MaximizeCommand = new RelayCommand(_ =>
        {
            WState = WState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
        });

        MinimizeCommand = new RelayCommand(_ =>
        {
            WState = WState == WindowState.Minimized ? WindowState.Normal : WindowState.Minimized;
        });

        StartViewModelCommand = new RelayCommand(_ =>
        {
            CurrentView = StartVm;
        });

        DummyViewModelCommand = new RelayCommand(_ =>
        {
            CurrentView = DummyVm;
        });
    }
}

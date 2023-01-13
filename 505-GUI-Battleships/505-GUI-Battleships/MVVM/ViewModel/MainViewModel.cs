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

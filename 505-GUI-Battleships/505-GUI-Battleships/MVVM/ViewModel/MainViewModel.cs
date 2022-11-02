using _505_GUI_Battleships.Core;

namespace _505_GUI_Battleships.MVVM.ViewModel;

internal class MainViewModel
{
    public RelayCommand StartViewModelCommand { get; internal set; }

    public IChildViewModel CurrentView { get; set; }

    public MainViewModel()
    {
        CurrentView = new StartViewModel(this);

        StartViewModelCommand = new RelayCommand(_ => CurrentView = new StartViewModel(this));
    }
}

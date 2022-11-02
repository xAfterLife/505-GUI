using _505_GUI_Battleships.Core;

namespace _505_GUI_Battleships.MVVM.ViewModel;

internal class StartViewModel : IChildViewModel
{
    public RelayCommand DummyViewModelCommand { get; internal set; }

    public StartViewModel(MainViewModel parentViewModel)
    {
        ParentViewModel = parentViewModel;
        DummyViewModelCommand = new RelayCommand(_ => ParentViewModel.CurrentView = new StartViewModel(ParentViewModel));
    }

    public MainViewModel ParentViewModel { get; internal set; }
}

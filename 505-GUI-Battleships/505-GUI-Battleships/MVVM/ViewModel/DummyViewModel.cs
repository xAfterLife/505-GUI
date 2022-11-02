namespace _505_GUI_Battleships.MVVM.ViewModel;

internal class DummyViewModel : IChildViewModel
{
    public DummyViewModel(MainViewModel parentViewModel)
    {
        ParentViewModel = parentViewModel;
    }

    public MainViewModel ParentViewModel { get; internal set; }
}

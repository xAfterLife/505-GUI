namespace _505_GUI_Battleships.MVVM.ViewModel;

internal class MainViewModel
{
    public object CurrentView { get; set; }

    public MainViewModel()
    {
        CurrentView = new StartViewModel();
    }
}

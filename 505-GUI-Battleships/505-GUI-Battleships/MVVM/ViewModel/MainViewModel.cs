using System.Windows;
using _505_GUI_Battleships.Services;

namespace _505_GUI_Battleships.MVVM.ViewModel;

internal class MainViewModel
{
    private readonly App? _mainApp;
    public object CurrentView { get; set; }

    public MainViewModel()
    {
        _mainApp = Application.Current as App;
        CurrentView = new StartViewModel();

        //Beispiel wie man Services erstellt und den Log-Service Attached
        var eloService = new EloService();
        eloService.AttachLogger(_mainApp?.LogService!);
    }
}

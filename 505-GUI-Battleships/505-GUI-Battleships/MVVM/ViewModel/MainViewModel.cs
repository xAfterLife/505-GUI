using System.Windows;
using _505_GUI_Battleships.Services;

namespace _505_GUI_Battleships.MVVM.ViewModel;

internal class MainViewModel
{
    public object CurrentView { get; set; } = new StartViewModel();

    public MainViewModel()
    {
        var mainApp = Application.Current as App;

        //Beispiel wie man Services erstellt und den Log-Service Attached
        var eloService = new EloService();
        eloService.AttachLogger(mainApp?.LogService!);
    }
}

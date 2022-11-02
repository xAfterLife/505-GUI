using System.Windows;
using _505_GUI_Battleships.Services;

namespace _505_GUI_Battleships;

/// <summary>
///     Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    public LogService LogService;

    public App()
    {
#if DEBUG
        LogService = new LogService(LogService.OutputType.MessageBox, LogService.FilterSeverity.Extended);
#else
        LogService = new LogService(LogService.OutputType.LogFile, LogService.FilterSeverity.Production, Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logfile.log"));
#endif
    }
}

using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using _505_GUI_Battleships.Services;

namespace _505_GUI_Battleships;

/// <summary>
///     Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    public bool IsDebugged;
    public LogService LogService;

    public App()
    {
        IsDebugged = Debugger.IsAttached;

        if ( IsDebugged )
            LogService = new LogService(LogService.OutputType.MessageBox, LogService.FilterSeverity.All);
        else
            LogService = new LogService(LogService.OutputType.LogFile, LogService.FilterSeverity.Production, Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logfile.log"));
    }
}

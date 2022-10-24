using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using _505_GUI_Battleships.Types;
using ErrorEventArgs = _505_GUI_Battleships.Types.ErrorEventArgs;

namespace _505_GUI_Battleships.Services;

public sealed class LogService
{
    public enum FilterSeverity
    {
        All,
        NoDebug,
        Extended,
        Production,
        None
    }

    public enum OutputType
    {
        None,
        Console,
        LogFile,
        MessageBox,
        All
    }

    public enum Severity
    {
        Debug = ConsoleColor.DarkBlue,
        Info = ConsoleColor.DarkGreen,
        Warning = ConsoleColor.DarkYellow,
        Error = ConsoleColor.DarkRed
    }

    private readonly OutputType _debugType;

    private readonly FilterSeverity _filterSeverity;

    private readonly string? _logPath;

    public LogService(OutputType outputType, FilterSeverity filterSeverity)
    {
        _debugType = outputType;
        _filterSeverity = filterSeverity;
    }

    public LogService(OutputType outputType, FilterSeverity filterSeverity, string logPath)
    {
        _debugType = outputType;
        _logPath = logPath;
        _filterSeverity = filterSeverity;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining & MethodImplOptions.AggressiveOptimization)]
    public void Debug(string message, [CallerMemberName] string caller = "", [CallerFilePath] string file = "", [CallerLineNumber] int line = 0)
    {
        Log(Severity.Debug, message, caller, file, line);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining & MethodImplOptions.AggressiveOptimization)]
    public void Info(string message, [CallerMemberName] string caller = "", [CallerFilePath] string file = "", [CallerLineNumber] int line = 0)
    {
        Log(Severity.Info, message, caller, file, line);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining & MethodImplOptions.AggressiveOptimization)]
    public void Warning(string message, [CallerMemberName] string caller = "", [CallerFilePath] string file = "", [CallerLineNumber] int line = 0)
    {
        Log(Severity.Warning, message, caller, file, line);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining & MethodImplOptions.AggressiveOptimization)]
    public void Error(Exception? ex)
    {
        if ( ex == null )
            return;

        var st = new StackTrace(ex, true);
        var sf = st.GetFrame(st.FrameCount);

        Log(Severity.Error, $"{ex.GetType().FullName} - {ex.Message}{Environment.NewLine}{ex.StackTrace}", sf!.GetMethod()!.Name, sf.GetFileName()!, sf.GetFileLineNumber());
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining & MethodImplOptions.AggressiveOptimization)]
    private static bool ShouldLog(in Severity severity, in FilterSeverity filterSeverity)
    {
        return filterSeverity switch
        {
            FilterSeverity.All        => true,
            FilterSeverity.NoDebug    => severity is not Severity.Debug,
            FilterSeverity.Extended   => severity is Severity.Warning or Severity.Error,
            FilterSeverity.Production => severity is Severity.Error,
            FilterSeverity.None       => false,
            _                         => throw new ArgumentOutOfRangeException(nameof(filterSeverity), filterSeverity, null)
        };
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining & MethodImplOptions.AggressiveOptimization)]
    private void Log(Severity severity, string message = "", [CallerMemberName] string caller = "", [CallerFilePath] string file = "", [CallerLineNumber] int line = 0)
    {
        if ( string.IsNullOrEmpty(message) || _debugType == OutputType.None || !ShouldLog(in severity, in _filterSeverity) )
            return;

        if ( _debugType is OutputType.Console or OutputType.All )
        {
            Console.ForegroundColor = (ConsoleColor)severity;
            Console.Write($@"{DateTime.Now.ToLongTimeString()} [{Path.GetFileNameWithoutExtension(file)}->{caller} L{line}] ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write($@"{message}{Environment.NewLine}");
        }

        if ( _logPath != null && _debugType is OutputType.LogFile or OutputType.All )
            File.WriteAllText(_logPath, $@"[{Path.GetFileNameWithoutExtension(file)}->{caller} L{line}] {message}");

        if ( _debugType is OutputType.MessageBox or OutputType.All )
            MessageBox.Show($@"[{Path.GetFileNameWithoutExtension(file)}->{caller} L{line}] {message}", Enum.GetName(typeof(Severity), severity), MessageBoxButton.OK, severity switch
            {
                Severity.Debug   => MessageBoxImage.Question,
                Severity.Info    => MessageBoxImage.Information,
                Severity.Warning => MessageBoxImage.Warning,
                Severity.Error   => MessageBoxImage.Error,
                _                => throw new ArgumentOutOfRangeException(nameof(severity), severity, null)
            });
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining & MethodImplOptions.AggressiveOptimization)]
    public void OnLogEventHandler(object? sender, LogMessageEventArgs e)
    {
        var logMessage = e.LogMessage;
        Log(logMessage.Severity, logMessage.Message, logMessage.Caller, logMessage.File, logMessage.Line);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining & MethodImplOptions.AggressiveOptimization)]
    public void OnErrorEventHandler(object? sender, ErrorEventArgs e)
    {
        Error(e.Exception);
    }
}

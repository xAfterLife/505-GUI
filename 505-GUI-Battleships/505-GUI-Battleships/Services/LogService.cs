using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using _505_GUI_Battleships.Structs;

// ReSharper disable UnusedMember.Global

namespace _505_GUI_Battleships.Services;

public sealed class LogService
{
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

    private readonly string? _logPath;

    public LogService(OutputType outputType)
    {
        _debugType = outputType;
    }

    public LogService(OutputType outputType, string logPath)
    {
        _debugType = outputType;
        _logPath = logPath;
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
    private void Log(Severity severity, string message = "", [CallerMemberName] string caller = "", [CallerFilePath] string file = "", [CallerLineNumber] int line = 0)
    {
        if ( string.IsNullOrEmpty(message) || _debugType == OutputType.None )
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
        Log(e.LogMessage);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining & MethodImplOptions.AggressiveOptimization)]
    private void Log(LogMessage logMessage)
    {
        if ( string.IsNullOrEmpty(logMessage.Message) || _debugType == OutputType.None )
            return;

        if ( _debugType is OutputType.Console or OutputType.All )
        {
            Console.ForegroundColor = (ConsoleColor)logMessage.Severity;
            Console.Write($@"{DateTime.Now.ToLongTimeString()} [{Path.GetFileNameWithoutExtension(logMessage.File)}->{logMessage.Caller} L{logMessage.Line}] ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write($@"{logMessage.Message}{Environment.NewLine}");
        }

        if ( _logPath != null && _debugType is OutputType.LogFile or OutputType.All )
            File.WriteAllText(_logPath, $@"[{Path.GetFileNameWithoutExtension(logMessage.File)}->{logMessage.Caller} L{logMessage.Line}] {logMessage.Message}");

        if ( _debugType is OutputType.MessageBox or OutputType.All )
            MessageBox.Show($@"[{Path.GetFileNameWithoutExtension(logMessage.File)}->{logMessage.Caller} L{logMessage.Line}] {logMessage.Message}", Enum.GetName(typeof(Severity), logMessage.Severity), MessageBoxButton.OK, logMessage.Severity switch
            {
                Severity.Debug   => MessageBoxImage.Question,
                Severity.Info    => MessageBoxImage.Information,
                Severity.Warning => MessageBoxImage.Warning,
                Severity.Error   => MessageBoxImage.Error,
                _                => throw new ArgumentOutOfRangeException(nameof(logMessage.Severity), logMessage.Severity, null)
            });
    }
}

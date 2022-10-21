using System;
using System.Runtime.CompilerServices;
using _505_GUI_Battleships.Services;

namespace _505_GUI_Battleships.Structs;

public readonly struct LogMessage
{
    public readonly LogService.Severity Severity;
    public readonly string Message;
    public readonly string Caller;
    public readonly string File;
    public readonly int Line;

    public LogMessage(LogService.Severity severity, string message = "", [CallerMemberName] string caller = "", [CallerFilePath] string file = "", [CallerLineNumber] int line = 0)
    {
        Severity = severity;
        Message = message;
        Caller = caller;
        File = file;
        Line = line;
    }
}

public sealed class LogMessageEventArgs : EventArgs
{
    public readonly LogMessage LogMessage;

    [MethodImpl(MethodImplOptions.AggressiveInlining & MethodImplOptions.AggressiveOptimization)]
    public LogMessageEventArgs(LogMessage logMessage)
    {
        LogMessage = logMessage;
    }
}

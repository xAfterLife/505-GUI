using System;
using _505_GUI_Battleships.Types;

namespace _505_GUI_Battleships.Services;

public class ServiceBase
{
    public event EventHandler<LogMessageEventArgs>? Log;
    public event EventHandler<ErrorEventArgs>? Error;

    public void AttachLogger(LogService logService)
    {
        Log += logService.OnLogEventHandler;
        Error += logService.OnErrorEventHandler;
        OnLog(new LogMessage(LogService.Severity.Debug, $"{GetType().Name} Logger-Attached"));
    }

    internal void OnLog(LogMessage e)
    {
        Log?.Invoke(this, new LogMessageEventArgs(e));
    }

    // ReSharper disable once UnusedMember.Local
    internal void OnError(Exception e)
    {
        Error?.Invoke(this, new ErrorEventArgs(e));
    }
}

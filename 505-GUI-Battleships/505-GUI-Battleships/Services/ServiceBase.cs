using System;
using _505_GUI_Battleships.Types;

namespace _505_GUI_Battleships.Services;

public class ServiceBase
{
    public event EventHandler<LogMessageEventArgs>? Log;

    public void AttachLogger(LogService logService)
    {
        Log += logService.OnLogEventHandler;
        OnLog(new LogMessage(LogService.Severity.Debug, $"{GetType().Name} Logger-Attached"));
    }

    // ReSharper disable once UnusedMember.Local
    private void OnLog(LogMessage e)
    {
        Log?.Invoke(this, new LogMessageEventArgs(e));
    }
}

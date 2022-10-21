using System;
using _505_GUI_Battleships.Structs;

namespace _505_GUI_Battleships.Services;

public class ServiceBase
{
    public event EventHandler<LogMessageEventArgs>? Log;

    public void AttachLogger(ref LogService logService)
    {
        Log += logService.OnLogEventHandler;
    }

    private void OnLog(LogMessageEventArgs e)
    {
        Log?.Invoke(this, e);
    }
}

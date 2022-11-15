using System;
using _505_GUI_Battleships.Types;

namespace _505_GUI_Battleships.Services;

public class ServiceBase
{
    /// <summary>
    ///     Event for Logging
    /// </summary>
    public event EventHandler<LogMessageEventArgs>? Log;

    /// <summary>
    ///     Event for Errors
    /// </summary>
    public event EventHandler<ErrorEventArgs>? Error;

    /// <summary>
    ///     Set's up Log- and Error-Handling
    /// </summary>
    /// <param name="logService">LogService</param>
    public void AttachLogger(LogService logService)
    {
        Log += logService.OnLogEventHandler;
        Error += logService.OnErrorEventHandler;
        OnLog(new LogMessage(LogService.Severity.Debug, $"{GetType().Name} Logger-Attached"));
    }

    /// <summary>
    ///     Global Handler for Log-Messages
    /// </summary>
    /// <param name="e">LogMessage</param>
    internal void OnLog(LogMessage e)
    {
        Log?.Invoke(this, new LogMessageEventArgs(e));
    }

    // ReSharper disable once UnusedMember.Local
    /// <summary>
    ///     Global Handler for Error-Handling
    /// </summary>
    /// <param name="e">Exception</param>
    internal void OnError(Exception e)
    {
        Error?.Invoke(this, new ErrorEventArgs(e));
    }
}

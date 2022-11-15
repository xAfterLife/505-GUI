using System;
using System.Runtime.CompilerServices;

namespace _505_GUI_Battleships.Types;

public sealed class LogMessageEventArgs : EventArgs
{
    public readonly LogMessage LogMessage;

    [MethodImpl(MethodImplOptions.AggressiveInlining & MethodImplOptions.AggressiveOptimization)]
    public LogMessageEventArgs(LogMessage logMessage)
    {
        LogMessage = logMessage;
    }
}

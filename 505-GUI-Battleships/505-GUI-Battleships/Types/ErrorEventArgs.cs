using System;
using System.Runtime.CompilerServices;

namespace _505_GUI_Battleships.Types;

public sealed class ErrorEventArgs : EventArgs
{
    public readonly Exception Exception;

    [MethodImpl(MethodImplOptions.AggressiveInlining & MethodImplOptions.AggressiveOptimization)]
    public ErrorEventArgs(Exception exception)
    {
        Exception = exception;
    }
}

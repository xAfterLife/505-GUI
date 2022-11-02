using System;
using System.Windows.Input;

namespace _505_GUI_Battleships.Core;

internal class RelayCommand : ICommand
{
    private readonly Action<object?> _method;

    public RelayCommand(Action<object?> method)
    {
        _method = method;
    }

    public event EventHandler? CanExecuteChanged;

    public bool CanExecute(object? parameter)
    {
        return true;
    }

    public void Execute(object? parameter)
    {
        _method.Invoke(parameter);
    }
}

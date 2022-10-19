using System;
using System.Windows.Input;

namespace _505_GUI_Battleships.Core;

internal class RelayCommand : ICommand
{
    private readonly Func<object, bool> _canExecute;
    private readonly Action<object> _execute;

    public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null!)
    {
        _execute = execute;
        _canExecute = canExecute;
    }

    public event EventHandler? CanExecuteChanged
    {
        add => CommandManager.RequerySuggested += value;
        remove => CommandManager.RequerySuggested -= value;
    }

    public bool CanExecute(object? parameter)
    {
        return parameter != null && _canExecute(parameter);
    }

    public void Execute(object? parameter)
    {
        if ( parameter != null )
            _execute(parameter);
    }
}

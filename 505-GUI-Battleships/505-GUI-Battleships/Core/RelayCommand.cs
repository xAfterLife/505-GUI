﻿using System;
using System.Windows.Input;

namespace _505_GUI_Battleships.Core;

public class RelayCommand : ICommand
{
    private readonly Action<object?> _action;

    public RelayCommand(Action<object?> action)
    {
        _action = action;
    }

    public void Execute(object? parameter)
    {
        _action(parameter);
    }

    public bool CanExecute(object? parameter)
    {
        return true;
    }

    public event EventHandler? CanExecuteChanged
    {
        add {}
        remove {}
    }
}

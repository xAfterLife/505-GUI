﻿using System.Windows;
using System.Windows.Input;
using _505_GUI_Battleships.Core;

namespace _505_GUI_Battleships.MVVM.ViewModel;

internal class StartViewModel : ObservableObject
{
    public static ICommand PlayerConfigurationViewModel => new RelayCommand(_ => ChangeViewModel.ChangeView(ChangeViewModel.ViewType.PlayerConfiguration));

    public static ICommand ExitCommand => new RelayCommand(_ => Application.Current.Shutdown());
}

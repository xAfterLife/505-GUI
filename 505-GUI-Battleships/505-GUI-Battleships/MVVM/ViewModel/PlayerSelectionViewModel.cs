using System.Collections.ObjectModel;
using System.Windows.Documents;
using System.Windows.Input;
using _505_GUI_Battleships.Core;
using _505_GUI_Battleships.MVVM.Model;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using System.Linq;
using System.Windows.Controls;
using System.Windows;
using System;

namespace _505_GUI_Battleships.MVVM.ViewModel;

internal class PlayerSelectionViewModel : ObservableObject
{
    public ObservableCollection<PlayerModel> Players { get; set; }

    public Visibility AddPlayerButtonVisibility
    {
        get => Players.Count >= 4 ? Visibility.Hidden : Visibility.Visible;
    }
    public static ICommand? AddPlayer { get; set; }

    public static ICommand? DeletePlayer { get; set; }


    public PlayerSelectionViewModel()
    {
        Players = new ObservableCollection<PlayerModel> { new PlayerModel(), new PlayerModel() };

        DeletePlayer = new RelayCommand(_ =>
        {
            Players.RemoveAt(Players.Count() - 1);
        });

        AddPlayer = new RelayCommand(_ =>
        {
            Players.Add(new PlayerModel());
            OnPropertyChanged("AddPlayerButtonVisibility");
        });
    }
}
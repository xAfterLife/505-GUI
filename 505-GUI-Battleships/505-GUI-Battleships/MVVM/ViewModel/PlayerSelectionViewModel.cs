using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using _505_GUI_Battleships.Core;
using _505_GUI_Battleships.MVVM.Model;

namespace _505_GUI_Battleships.MVVM.ViewModel;

internal sealed class PlayerSelectionViewModel : ObservableObject
{
    /// <summary>
    ///     Collection of Players from which the PlayerSelectionCards are created
    /// </summary>
    public ObservableCollection<PlayerModel> Players { get; set; }

    /// <summary>
    ///     Binding for the AddPlayerButton
    /// </summary>
    public Visibility AddPlayerButtonVisibility => Players.Count >= 4 ? Visibility.Hidden : Visibility.Visible;
   
    /// <summary>
    ///     AddPlayer Command
    /// </summary>
    public static ICommand? AddPlayer { get; set; }

    /// <summary>
    ///     SelectModeButton Command
    /// </summary>
    public static ICommand? SelectModeButton { get; set; }
    
    /// <summary>
    ///     Backbutton Command
    /// </summary>
    public static ICommand? BackButton { get; set; }


    /// <summary>
    ///     ctor
    /// </summary>
    public PlayerSelectionViewModel()
    {
        Players = new ObservableCollection<PlayerModel> { new(), new() };

        for (int i = 0; i < Players.Count; i++)
        {
            Players[i].DeleteButtonVisibility = Visibility.Hidden;
        }

        //Subscribe to DeleteButtonPressed and Remove the Players from the List
        PlayerModel.DeleteButtonPressed += (sender, _) =>
        {
            if ( sender is PlayerModel player ) Players.Remove(player);
            if (Players.Count <= 2)
            {
                for (int i = 0; i < Players.Count; i++)
                {
                    Players[i].DeleteButtonVisibility = Visibility.Hidden;
                    Players[i].UpdateDeleteButton();
                }
            }
            OnPropertyChanged(nameof(AddPlayerButtonVisibility));
        };

        AddPlayer = new RelayCommand(_ =>
        {
            Players.Add(new PlayerModel());
            if (Players.Count > 2)
            {
                for (int i = 0; i < Players.Count; i++)
                {
                    Players[i].DeleteButtonVisibility = Visibility.Visible;
                    Players[i].UpdateDeleteButton();
                }
            }
            OnPropertyChanged(nameof(AddPlayerButtonVisibility));
        });

        SelectModeButton = new RelayCommand(_ => 
        {
            foreach (PlayerModel player in Players)
            {
                Trace.WriteLine(player.PlayerName);
                Trace.WriteLine(player.PlayerColor);
            }
        });

        BackButton = new RelayCommand(_ => ChangeViewModel.ChangeView(ChangeViewModel.ViewType.Start));
    }
}

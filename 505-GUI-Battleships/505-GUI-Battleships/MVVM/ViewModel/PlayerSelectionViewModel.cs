using System.Collections.ObjectModel;
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
    ///     ctor
    /// </summary>
    public PlayerSelectionViewModel()
    {
        Players = new ObservableCollection<PlayerModel> { new(), new() };

        //Subscribe to DeleteButtonPressed and Remove the Players from the List
        PlayerModel.DeleteButtonPressed += (sender, _) =>
        {
            if ( sender is PlayerModel player )
                Players.Remove(player);
        };

        AddPlayer = new RelayCommand(_ =>
        {
            Players.Add(new PlayerModel());
            OnPropertyChanged(nameof(AddPlayerButtonVisibility));
        });
    }
}

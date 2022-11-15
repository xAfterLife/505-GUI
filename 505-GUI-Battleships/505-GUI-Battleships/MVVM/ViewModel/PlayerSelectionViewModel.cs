using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using _505_GUI_Battleships.Core;
using _505_GUI_Battleships.MVVM.Model;

namespace _505_GUI_Battleships.MVVM.ViewModel;

internal sealed class PlayerSelectionViewModel : ObservableObject
{
    public ObservableCollection<PlayerModel> Players { get; set; }
    public Visibility AddPlayerButtonVisibility => Players.Count >= 4 ? Visibility.Hidden : Visibility.Visible;
    public static ICommand? AddPlayer { get; set; }

    public PlayerSelectionViewModel()
    {
        Players = new ObservableCollection<PlayerModel> { new(), new() };

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

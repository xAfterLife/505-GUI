using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using _505_GUI_Battleships.Core;
using _505_GUI_Battleships.MVVM.Model;
using _505_GUI_Battleships.Services;

namespace _505_GUI_Battleships.MVVM.ViewModel;

internal sealed class PlayerSelectionViewModel : ObservableObject
{
    private readonly GameDataService _gameService;

    /// <summary>
    ///     Collection of Players from which the PlayerSelectionCards are created
    /// </summary>
    public ObservableCollection<PlayerModel> Players { get; set; }

    /// <summary>
    ///     Binding for the AddPlayerButton
    /// </summary>
    public Visibility AddPlayerButtonVisibility => Players.Count >= 4 ? Visibility.Collapsed : Visibility.Visible;

    /// <summary>
    ///     AddPlayer Command
    /// </summary>
    public static ICommand? AddPlayerCommand { get; set; }

    /// <summary>
    ///     GoToGameOptionsCommand Command
    /// </summary>
    public static ICommand? GoToGameOptionsCommand { get; set; }

    /// <summary>
    ///     Backbutton Command
    /// </summary>
    public static ICommand? BackCommand { get; set; }

    /// <summary>
    ///     ctor
    /// </summary>
    public PlayerSelectionViewModel()
    {
        _gameService = GameDataService.GetInstance();
        if ( !_gameService.PlayerModels.Any() )
            Players = new ObservableCollection<PlayerModel> { new(), new() };
        else
            Players = _gameService.PlayerModels;

        if ( Players.Count <= 2 )
            foreach ( var playerModel in Players )
                playerModel.DeleteButtonVisibility = Visibility.Hidden;

        //Subscribe to DeleteButtonPressed and Remove the Players from the List
        PlayerModel.DeleteButtonPressed += (sender, _) =>
        {
            if ( sender is PlayerModel player )
                Players.Remove(player);

            if ( Players.Count <= 2 )
                foreach ( var playerModel in Players )
                {
                    playerModel.DeleteButtonVisibility = Visibility.Hidden;
                    playerModel.UpdateDeleteButton();
                }

            OnPropertyChanged(nameof(AddPlayerButtonVisibility));
        };

        AddPlayerCommand = new RelayCommand(_ =>
        {
            Players.Add(new PlayerModel());

            if ( Players.Count > 2 )
                foreach ( var playerModel in Players )
                {
                    playerModel.DeleteButtonVisibility = Visibility.Visible;
                    playerModel.UpdateDeleteButton();
                }

            OnPropertyChanged(nameof(AddPlayerButtonVisibility));
        });

        GoToGameOptionsCommand = new RelayCommand(_ =>
        {
            _gameService.PlayerModels = Players;
            ChangeViewModel.ChangeView(ChangeViewModel.ViewType.GameOptions);
        });

        BackCommand = new RelayCommand(_ =>
        {
            _gameService.PlayerModels.Clear();
            ChangeViewModel.ChangeView(ChangeViewModel.ViewType.Start);
        });
    }
}

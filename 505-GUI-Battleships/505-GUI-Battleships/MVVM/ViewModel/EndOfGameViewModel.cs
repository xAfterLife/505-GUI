using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using _505_GUI_Battleships.Core;
using _505_GUI_Battleships.MVVM.Model;
using _505_GUI_Battleships.Services;

namespace _505_GUI_Battleships.MVVM.ViewModel;

internal sealed class EndOfGameViewModel : ObservableObject, IDisposable
{
    /// <summary>
    ///     The List of all Players to Dispay their Playercards (Alive and Eliminated)
    /// </summary>
    public ObservableCollection<PlayerModel> Players { get; set; }

    /// <summary>
    ///     Command to get back to the Start-Screen
    /// </summary>
    public ICommand ReturnToStartCommand => new RelayCommand(_ => ChangeViewModel.ChangeView(ChangeViewModel.ViewType.Start, this));

    /// <summary>
    ///     Constructor of the EndOfGameViewModel
    /// </summary>
    public EndOfGameViewModel()
    {
        Players = new ObservableCollection<PlayerModel>();
        var gameService = GameDataService.GetInstance();
        foreach ( var player in gameService.PlayerModels )
            Players.Add(player);
        foreach ( var player in gameService.EliminatedPlayers )
            Players.Add(player);
        Players = new ObservableCollection<PlayerModel>(Players.OrderByDescending(player => player.Points));
        if ( !Players.Any(player => player.Winner) )
            Players[0].Winner = true;
    }

    /// <summary>
    ///     Dispose the Current ViewModel and Reset the Instance of the GameDataService to start a new Game
    /// </summary>
    public void Dispose()
    {
        GameDataService.GetInstance().ResetInstance();
    }
}

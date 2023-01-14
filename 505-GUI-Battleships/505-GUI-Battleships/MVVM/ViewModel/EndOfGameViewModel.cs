using System;
using System.Collections.ObjectModel;
using System.Linq;
using _505_GUI_Battleships.Core;
using _505_GUI_Battleships.MVVM.Model;
using _505_GUI_Battleships.Services;

namespace _505_GUI_Battleships.MVVM.ViewModel;

internal sealed class EndOfGameViewModel : ObservableObject, IDisposable
{
    public ObservableCollection<PlayerModel> Players { get; set; }

    public EndOfGameViewModel()
    {
        Players = new ObservableCollection<PlayerModel>();
        var gameservice = GameDataService.GetInstance();
        foreach ( var player in gameservice.PlayerModels )
            Players.Add(player);
        foreach ( var player in gameservice.EliminatedPlayers )
            Players.Add(player);
        Players = new ObservableCollection<PlayerModel>(Players.OrderByDescending(player => player.Points));
    }

    public void Dispose() {}
}

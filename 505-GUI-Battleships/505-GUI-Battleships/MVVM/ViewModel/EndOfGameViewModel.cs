using _505_GUI_Battleships.Core;
using _505_GUI_Battleships.MVVM.Model;
using _505_GUI_Battleships.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _505_GUI_Battleships.MVVM.ViewModel;

internal sealed class EndOfGameViewModel : ObservableObject, IDisposable
{
    public void Dispose()
    {
    }
    public ObservableCollection<PlayerModel> Players { get; set; }

    public EndOfGameViewModel()
    {
        Players = new ObservableCollection<PlayerModel>();
        var gameservice = GameDataService.GetInstance();
        foreach(var player in gameservice.PlayerModels)
            Players.Add(player);
        foreach (var player in gameservice.EliminatedPlayers)
            Players.Add(player);
        Players = new ObservableCollection<PlayerModel>(Players.OrderByDescending(player => player.Points));
    }
}

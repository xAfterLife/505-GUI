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

    public ICommand ReturnToStartCommand => new RelayCommand(_ =>
    {
        GameDataService.GetInstance().ResetInstance();
        ChangeViewModel.ChangeView(ChangeViewModel.ViewType.Start, this);
    });

    public EndOfGameViewModel()
    {
        Players = new ObservableCollection<PlayerModel>();
        var _gameService = GameDataService.GetInstance();
        foreach(var player in _gameService.PlayerModels)
            Players.Add(player);
        foreach (var player in _gameService.EliminatedPlayers)
            Players.Add(player);
        Players = new ObservableCollection<PlayerModel>(Players.OrderByDescending(player => player.Points));
        if (!Players.Any(player => player.Winner))
            Players[0].Winner = true;
    }

    public void Dispose() {}
}

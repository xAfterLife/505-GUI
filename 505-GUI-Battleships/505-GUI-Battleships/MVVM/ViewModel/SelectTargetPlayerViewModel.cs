using System.Collections.ObjectModel;
using System.Windows.Controls;
using _505_GUI_Battleships.Core;
using _505_GUI_Battleships.MVVM.Model;
using _505_GUI_Battleships.Services;

namespace _505_GUI_Battleships.MVVM.ViewModel;

internal sealed class SelectTargetPlayerViewModel : ObservableObject
{
    private readonly GameDataService _gameService;

    public string SelectTargetPlayerHeading;

    public ObservableCollection<PlayerModel> TargetablePlayers;

    private Grid _playerBoardPreview;

    public Grid PlayerBoardPreview
    {
        get => _playerBoardPreview;
        set => Update(ref _playerBoardPreview, value);
    }

    private string _roundCountText;

    public string RoundCountText
    {
        get => _roundCountText;
        set => Update(ref _roundCountText, value);
    } 

    /*No game-loop currently in place, therefore  player[0] serves as placeholder*/
    public SelectTargetPlayerViewModel()
    {
        _gameService = GameDataService.GetInstance();
        
        TargetablePlayers = _gameService.PlayerModels;
        //SelectTargetPlayerHeading = $"It's your turn to attack, {_gameService.PlayerModels[0].PlayerName}";
        RoundCountText = "1";
        SelectTargetPlayerHeading = "It's your turn to attack, Player";
    }
}

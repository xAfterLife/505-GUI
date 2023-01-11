using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Controls;
using _505_GUI_Battleships.Core;
using _505_GUI_Battleships.MVVM.Model;
using _505_GUI_Battleships.Services;

namespace _505_GUI_Battleships.MVVM.ViewModel;

internal sealed class SelectTargetPlayerViewModel : ObservableObject
{
    private readonly GameDataService _gameService;

    public string SelectTargetPlayerHeading;

    public ObservableCollection<PlayerModel> TargetablePlayers {get; set; }

    public ObservableCollection<SelectTargetPlayerCardModel> TargetElements { get; set; }

    private Canvas _playerBoard;
    public Canvas PlayerBoard
    {
        get => _playerBoard;
        set => Update(ref _playerBoard, value);
    }

    //private int _currentPlayer { get; set; }
    //private int _currentPlayerCounter;

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

        TargetElements = new ObservableCollection<SelectTargetPlayerCardModel>();
        //_currentPlayer = _gameService.PlayerModels.Count;
        SelectTargetPlayerHeading = $"It's your turn to attack, X";
        RoundCountText = "1";
        _playerBoard = _gameService.GameBoard.Board;
    }
}

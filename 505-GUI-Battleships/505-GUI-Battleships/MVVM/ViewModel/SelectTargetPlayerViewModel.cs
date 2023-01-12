using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using _505_GUI_Battleships.Core;
using _505_GUI_Battleships.MVVM.Model;
using _505_GUI_Battleships.Services;

namespace _505_GUI_Battleships.MVVM.ViewModel;

internal sealed class SelectTargetPlayerViewModel : ObservableObject
{
    private readonly GameDataService _gameService;

    private string _selectTargetPlayerHeading;
    public string SelectTargetPlayerHeading
    {
        get => _selectTargetPlayerHeading;
        set => Update(ref _selectTargetPlayerHeading, value);
    }

    public ObservableCollection<PlayerModel> TargetablePlayers {get; set; }

    private Canvas _playerBoard;
    public Canvas PlayerBoard
    {
        get => _playerBoard;
        set => Update(ref _playerBoard, value);
    }

    private PlayerModel _currentPlayer;
    public PlayerModel CurrentPlayer 
    { 
        get => _currentPlayer;
        set => Update(ref _currentPlayer, value);
    }

    private int _currentPlayerCounter { get; set; }

    private string _roundCountText;

    public string RoundCountText
    {
        get => _roundCountText;
        set => Update(ref _roundCountText, value);
    } 

    //TODO: Implement CurrentPlayer
    //TODO: Implement CurrentRound

    public SelectTargetPlayerViewModel()
    {
        _gameService = GameDataService.GetInstance();
        _currentPlayer = _gameService.CurrentPlayer;
        TargetablePlayers = _gameService.PlayerModels;
        TargetablePlayers.Remove(_currentPlayer);
        OnPropertyChanged(nameof(TargetablePlayers));

        SelectTargetPlayerHeading = $"It's your turn to attack, {_currentPlayer.PlayerName}!";
        // OnPropertyChanged(nameof(SelectTargetPlayerHeading));
        RoundCountText = _gameService.CurrentRound.ToString();
        _playerBoard = _gameService.GameBoard.Board;


        PlayerModel.SelectTargetPlayerCommandPressed += (sender, _) =>
        {
            //TODO: Load in actual Target
            if (sender is PlayerModel player)
                _gameService.CurrentTarget = player;
                ChangeViewModel.ChangeView(ChangeViewModel.ViewType.BoardAttack);
        };
    }
}

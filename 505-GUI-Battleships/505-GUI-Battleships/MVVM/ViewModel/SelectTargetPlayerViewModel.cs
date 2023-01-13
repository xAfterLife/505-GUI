using System;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using _505_GUI_Battleships.Core;
using _505_GUI_Battleships.MVVM.Model;
using _505_GUI_Battleships.Services;

namespace _505_GUI_Battleships.MVVM.ViewModel;

internal sealed class SelectTargetPlayerViewModel : ObservableObject, IDisposable
{
    private readonly GameDataService _gameService;

    private PlayerModel _currentPlayer;

    private Canvas _playerBoard;

    private string _roundCountText = string.Empty;

    private string _selectTargetPlayerHeading = string.Empty;

    public string SelectTargetPlayerHeading
    {
        get => _selectTargetPlayerHeading;
        set => Update(ref _selectTargetPlayerHeading, value);
    }

    public ObservableCollection<PlayerModel> TargetablePlayers { get; set; }

    public Canvas PlayerBoard
    {
        get => _playerBoard;
        set => Update(ref _playerBoard, value);
    }

    public PlayerModel CurrentPlayer
    {
        get => _currentPlayer;
        set => Update(ref _currentPlayer, value);
    }

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
        _currentPlayer = _gameService.CurrentPlayer!;
        TargetablePlayers = _gameService.PlayerModels;
        TargetablePlayers.Remove(_currentPlayer);

        SelectTargetPlayerHeading = $"It's your turn to attack, {_currentPlayer.PlayerName}!";
        RoundCountText = _gameService.CurrentRound.ToString();
        _playerBoard = _gameService.GameBoard!.Board;

        PlayerModel.SelectTargetPlayerCommandPressed += SelectTargetPlayerPressed;
    }

    public void Dispose()
    {
        PlayerModel.SelectTargetPlayerCommandPressed -= SelectTargetPlayerPressed;
    }

    private void SelectTargetPlayerPressed(object? sender, EventArgs args)
    {
        if ( sender is PlayerModel player )
            _gameService.CurrentTarget = player;
        ChangeViewModel.ChangeView(ChangeViewModel.ViewType.BoardAttack, this);
    }
}

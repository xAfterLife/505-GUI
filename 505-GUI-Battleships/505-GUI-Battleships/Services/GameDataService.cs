using System;
using System.Collections.ObjectModel;
using _505_GUI_Battleships.Core;
using _505_GUI_Battleships.MVVM.Model;

namespace _505_GUI_Battleships.Services;

internal class GameDataService
{
    private static GameDataService? _instance;
    private int _firstPlayerIndex;

    public GameOptionsModel? GameOptions { get; private set; }
    public GameBoardModel? GameBoard { get; private set; }
    public ObservableCollection<PlayerModel> PlayerModels { get; set; }
    public ObservableCollection<PlayerModel> EliminatedPlayers { get; set; }
    public ObservableCollection<ShipModel> ShipModels { get; set; }
    public PlayerModel? CurrentPlayer { get; set; }
    public int CurrentPlayerIndex { get; set; }
    public PlayerModel? CurrentTarget { get; set; }
    public int CurrentRound { get; set; }

    private GameDataService()
    {
        PlayerModels = new ObservableCollection<PlayerModel>();
        ShipModels = new ObservableCollection<ShipModel>();
        EliminatedPlayers = new ObservableCollection<PlayerModel>();
    }

    public static GameDataService GetInstance()
    {
        return _instance ??= new GameDataService();
    }

    public void Initialize(int boardHeight, int boardWidth, GameMode gameMode, int? rounds)
    {
        GameOptions = new GameOptionsModel(gameMode, rounds);
        GameBoard = new GameBoardModel(boardHeight, boardWidth);

        CurrentPlayerIndex = _firstPlayerIndex = Random.Shared.Next(PlayerModels.Count - 1);
        CurrentPlayer = PlayerModels[_firstPlayerIndex];
        CurrentRound = 1;

        foreach ( var player in PlayerModels )
            player.VisualPlayerBoard = new GameBoardModel(boardHeight, boardWidth).Board;
    }

    public void SetupPlayerShipModels()
    {
        foreach ( var player in PlayerModels )
            foreach ( var shipModel in ShipModels )
                player.Ships.Add(new ShipPlacementModel(-1, -1, true, shipModel.Id, -1));
    }

    public void SetCurrentTarget(PlayerModel currentTarget)
    {
        CurrentTarget = currentTarget;
    }

    public void SetNextPlayer()
    {
        if ( CurrentPlayerIndex >= PlayerModels.Count - 1 )
            CurrentPlayerIndex = 0;
        else
            CurrentPlayerIndex++;
        CurrentPlayer = PlayerModels[CurrentPlayerIndex];

        if ( CurrentPlayerIndex == _firstPlayerIndex )
            CurrentRound++;
    }

    public void PlayerKnockOut(IDisposable sender)
    {
        if ( GameOptions == null || CurrentTarget == null )
            throw new Exception("Service is not Initialized");
        if ( GameOptions.GameMode == GameMode.FirstOneOut )
            ChangeViewModel.ChangeView(ChangeViewModel.ViewType.EndOfGame, sender);

        int i;
        for ( i = 0; i < PlayerModels.Count; i++ )
            if ( PlayerModels[i] == CurrentTarget )
                break;

        if ( CurrentPlayerIndex > i )
            CurrentPlayerIndex--;

        EliminatedPlayers.Add(CurrentTarget);
        PlayerModels.Remove(CurrentTarget);

        if ( _firstPlayerIndex >= PlayerModels.Count )
            _firstPlayerIndex = 0;
    }

    public bool CheckGameOver()
    {
        return PlayerModels.Count == 1;
    }

    public void ResetInstance()
    {
        _instance = new GameDataService();
    }
}

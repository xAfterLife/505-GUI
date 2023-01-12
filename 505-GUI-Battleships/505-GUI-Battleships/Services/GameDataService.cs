using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Controls;
using _505_GUI_Battleships.MVVM.Model;

namespace _505_GUI_Battleships.Services;

internal class GameDataService : ServiceBase
{
    private static GameDataService? _instance;

    public GameOptionsModel? GameOptions { get; private set; }
    public GameBoardModel? GameBoard { get; private set; }
    public ObservableCollection<PlayerModel> PlayerModels { get; set; }
    public ObservableCollection<ShipModel> ShipModels { get; set; }
    
    public PlayerModel? CurrentPlayer { get; set; }
    public int CurrentPlayerIndex { get; set; }
    public PlayerModel? CurrentTarget { get; set; }
    public int CurrentRound { get; set; }

    private GameDataService()
    {
        PlayerModels = new ObservableCollection<PlayerModel>();
        ShipModels = new ObservableCollection<ShipModel>();
    }

    public static GameDataService GetInstance()
    {
        return _instance ??= new GameDataService();
    }

    public void Initialize(int boardHeight, int boardWidth, GameMode gameMode, int? rounds)
    {
        GameOptions = new GameOptionsModel(gameMode, rounds);
        GameBoard = new GameBoardModel(boardHeight, boardWidth);


        Random rInt = new();
        CurrentPlayerIndex = rInt.Next(0, PlayerModels.Count - 1);
        CurrentPlayer = PlayerModels[CurrentPlayerIndex];
        CurrentRound = 1;
        foreach (var player in PlayerModels)
        {
            player.VisualPlayerBoard = GameBoard.Board;
        }
    }

    public void SetupPlayerShipModels()
    {
        foreach ( var player in PlayerModels )
            foreach (ShipModel shipModel in ShipModels)
                player.Ships.Add(new ShipPlacementModel(-1, -1, true, shipModel.Id, -1));
    }

    public void SetCurrentTarget(PlayerModel currentTarget)
    {
        CurrentTarget = currentTarget;
    }

/*    public void SetNextPlayer() 
    {
        if( CurrentPlayerIndex == PlayerModels.Count - 1)
        {
            CurrentPlayerIndex = 0;
        } else CurrentPlayerIndex++;
        CurrentPlayer = PlayerModels[CurrentPlayerIndex];
    }*/

/*    public void PlayerKnockOut()
    {
        PlayerModels.Remove(CurrentTarget);
    }*/

/*    public bool CheckGameOver()
    {
        if(PlayerModels.Count < 2)
        {
            return true;
        }
        return false;
    }*/

}

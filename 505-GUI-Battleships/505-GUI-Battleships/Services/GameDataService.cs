using System.Collections.ObjectModel;
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
    public ObservableCollection<Canvas> VisualPlayerBoard { get; set; }

    private GameDataService()
    {
        PlayerModels = new ObservableCollection<PlayerModel>();
        ShipModels = new ObservableCollection<ShipModel>();
        VisualPlayerBoard = new ObservableCollection<Canvas>();
    }

    public static GameDataService GetInstance()
    {
        return _instance ??= new GameDataService();
    }

    public void Initialize(int boardHeight, int boardWidth, GameMode gameMode, int? rounds)
    {
        GameOptions = new GameOptionsModel(gameMode, rounds);
        GameBoard = new GameBoardModel(boardHeight, boardWidth);
        foreach (var player in PlayerModels)
        {
            VisualPlayerBoard.Add(GameBoard.Board);
        }
    }

    public void SetupPlayerShipModels()
    {
        foreach ( var player in PlayerModels )
            foreach (ShipModel shipModel in ShipModels)
                player.Ships.Add(new ShipPlacementModel(-1, -1, true, shipModel.Id, -1));
    }
}

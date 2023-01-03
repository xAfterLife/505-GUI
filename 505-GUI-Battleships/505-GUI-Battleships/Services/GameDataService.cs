using System.Collections.Generic;
using System.Collections.ObjectModel;
using _505_GUI_Battleships.MVVM.Model;

namespace _505_GUI_Battleships.Services;

internal class GameDataService : ServiceBase
{
    private static GameDataService? _instance;

    public GameOptionsModel? GameOptions { get; set; }
    public GameBoardModel? GameBoard { get; set; }
    public ObservableCollection<PlayerModel> PlayerModels { get; set; }
    public ObservableCollection<ShipModel> ShipModels { get; set; }

    private GameDataService()
    {
        PlayerModels = new ObservableCollection<PlayerModel>();
        ShipModels = new ObservableCollection<ShipModel>();
    }

    public static GameDataService GetInstance()
    {
        return _instance ??= new GameDataService();
    }

    public void Initialize(int boardHeight, int boardWidth, GameMode gameMode, int? rounds, List<int> shipLengthList)
    {
        GameOptions = new GameOptionsModel(boardHeight, boardWidth, gameMode, rounds, shipLengthList);
        GameBoard = new GameBoardModel(GameOptions.BoardHeight, GameOptions.BoardWidth);
    }
}

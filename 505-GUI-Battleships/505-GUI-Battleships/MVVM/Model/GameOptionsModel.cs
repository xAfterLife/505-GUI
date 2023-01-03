using System.Collections.Generic;

namespace _505_GUI_Battleships.MVVM.Model;

internal class GameOptionsModel
{
    public int BoardHeight { get; set; }
    public int BoardWidth { get; set; }
    public GameMode GameMode { get; set; }
    public int? Rounds { get; set; }
    public List<int> ShipLengthList { get; set; }

    public GameOptionsModel(int boardHeight, int boardWidth, GameMode gameMode, int? rounds, List<int> shipLengthList)
    {
        BoardHeight = boardHeight;
        BoardWidth = boardWidth;
        GameMode = gameMode;
        Rounds = rounds;
        ShipLengthList = shipLengthList;
    }
}

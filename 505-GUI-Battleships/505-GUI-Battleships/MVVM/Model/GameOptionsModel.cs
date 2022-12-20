namespace _505_GUI_Battleships.MVVM.Model;

public enum GameMode
{
    FirstToWin,
    LastManStanding,
    RoundsLimited
}

internal class GameOptionsModel
{
    public int BoardHeight { get; set; }
    public int BoardWidth { get; set; }
    public GameMode GameMode { get; set; }
    public int? Rounds { get; set; }

    public GameOptionsModel(int boardHeight, int boardWidth, GameMode gameMode, int? rounds)
    {
        BoardHeight = boardHeight;
        BoardWidth = boardWidth;
        GameMode = gameMode;
        Rounds = rounds;
    }
}

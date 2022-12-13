namespace _505_GUI_Battleships.MVVM.Model;

internal class GameOptinonsModel
{
    public int[][] BoardDimensions;
    public int GameMode;
    public int PlayerCount;
    public int? Rounds;
    public int ShipSquareCount;

    public GameOptinonsModel(int playerCount, int[][] boardDimensions, int? rounds, int shipSquareCount, int gameMode)
    {
        PlayerCount = playerCount;
        BoardDimensions = boardDimensions;
        Rounds = rounds;
        ShipSquareCount = shipSquareCount;
        GameMode = gameMode;
    }
}

namespace _505_GUI_Battleships.MVVM.Model;

internal class GameOptionsModel
{
    public GameMode GameMode { get; set; }
    public int? Rounds { get; set; }

    public GameOptionsModel(GameMode gameMode, int? rounds)
    {
        GameMode = gameMode;
        Rounds = rounds;
    }
}

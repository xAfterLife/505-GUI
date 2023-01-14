namespace _505_GUI_Battleships.MVVM.Model;

internal class GameOptionsModel
{
    /// <summary>
    ///     Enum of GameMode
    /// </summary>
    public GameMode GameMode { get; set; }

    /// <summary>
    ///     Number of Rounds till the Game ends
    /// </summary>
    public int? Rounds { get; set; }

    /// <summary>
    ///     Constructor of GameOptionsModel
    /// </summary>
    /// <param name="gameMode"></param>
    /// <param name="rounds">Can be null</param>
    public GameOptionsModel(GameMode gameMode, int? rounds)
    {
        GameMode = gameMode;
        Rounds = rounds;
    }
}

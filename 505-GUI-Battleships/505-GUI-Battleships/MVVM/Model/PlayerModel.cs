using _505_GUI_Battleships.Core;

namespace _505_GUI_Battleships.MVVM.Model;

public sealed class PlayerModel : ObservableObject
{
    private uint _elo;
    private string _playerName;

    public string PlayerName
    {
        get => _playerName;
        set => Update(ref _playerName, value);
    }

    public uint Elo
    {
        get => _elo;
        set => Update(ref _elo, value);
    }

    public PlayerModel(string playerName = "test", uint elo = 1000)
    {
        _playerName = playerName;
        _elo = elo;
    }
}

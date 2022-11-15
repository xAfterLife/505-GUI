using _505_GUI_Battleships.Core;
using System;
using System.Windows.Input;

namespace _505_GUI_Battleships.MVVM.Model;

public sealed class PlayerModel : ObservableObject
{
    private uint _elo;
    private string _playerName;
    private Guid _playerId;

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

    public Guid PlayerId
    {
        get => _playerId;
        set => Update(ref _playerId, value);
    }

    public PlayerModel(string playerName = "Enter name", uint elo = 1000)
    {
        _playerName = playerName;
        _elo = elo;
        _playerId = Guid.NewGuid();
    }
}

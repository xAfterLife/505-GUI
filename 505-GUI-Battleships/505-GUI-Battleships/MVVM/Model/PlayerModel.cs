using System;
using System.Windows.Input;
using _505_GUI_Battleships.Core;

namespace _505_GUI_Battleships.MVVM.Model;

public sealed class PlayerModel : ObservableObject
{
    private uint _elo;
    private Guid _playerId;
    private string _playerName;
    public PlayerModel Instance { get; }

    public static ICommand DeleteButtonCommand =>
        new RelayCommand(instance =>
        {
            if ( instance is not PlayerModel player )
                return;

            DeleteButtonPressed?.Invoke(player, EventArgs.Empty);
        });

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
        Instance = this;
        _playerName = playerName;
        _elo = elo;
        _playerId = Guid.NewGuid();
    }

    public static event EventHandler? DeleteButtonPressed;
}

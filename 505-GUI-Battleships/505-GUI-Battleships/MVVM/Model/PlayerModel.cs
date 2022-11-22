using System;
using System.Windows;
using System.Windows.Input;
using _505_GUI_Battleships.Core;

namespace _505_GUI_Battleships.MVVM.Model;

public sealed class PlayerModel : ObservableObject
{
    private uint _elo;
    private Guid _playerId;
    private string _playerName;

    /// <summary>
    ///     The Instance of PlayerModel used to pass as a Parameter to the static Command
    /// </summary>
    public PlayerModel Instance { get; }

    public Visibility DeleteButtonVisibility { get; set; }

    /// <summary>
    ///     Command for removing the pressed PlayerSelectionCard
    ///     -> Raises DeleteButtonPressed Event
    /// </summary>
    public static ICommand DeleteButtonCommand =>
        new RelayCommand(instance =>
        {
            if ( instance is not PlayerModel player )
                return;

            DeleteButtonPressed?.Invoke(player, EventArgs.Empty);
        });

    /// <summary>
    ///     Name of the Player
    /// </summary>
    public string PlayerName
    {
        get => _playerName;
        set => Update(ref _playerName, value);
    }

    /// <summary>
    ///     Elo-Points of the Player
    /// </summary>
    public uint Elo
    {
        get => _elo;
        set => Update(ref _elo, value);
    }

    /// <summary>
    ///     Guid as a Unique-ID for the Player
    /// </summary>
    public Guid PlayerId
    {
        get => _playerId;
        set => Update(ref _playerId, value);
    }

    /// <summary>
    /// </summary>
    /// <param name="playerName">Name of the Player</param>
    /// <param name="elo">Elo points of the Player</param>
    public PlayerModel(string playerName = "Enter name", uint elo = 1000)
    {
        Instance = this;
        _playerName = playerName;
        _elo = elo;
        _playerId = Guid.NewGuid();
    }

    public void UpdateDeleteButton() { 
        OnPropertyChanged(nameof(this.DeleteButtonVisibility));
    }

    /// <summary>
    ///     Event Handler for the DeleteButton
    /// </summary>
    public static event EventHandler? DeleteButtonPressed;
}

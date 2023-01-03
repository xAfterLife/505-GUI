using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using _505_GUI_Battleships.Core;

namespace _505_GUI_Battleships.MVVM.Model;

public sealed class PlayerModel : ObservableObject
{
    private uint _elo;
    private Color _playerColor;
    private Guid _playerId;
    private string _playerName;

    public ObservableCollection<PlayerAttackModel> Attacks { get; set; } = new();
    public ObservableCollection<ShipPlacementModel> Ships { get; set; } = new();

    /// <summary>
    ///     The Instance of PlayerModel used to pass as a Parameter to the static Command
    /// </summary>
    public PlayerModel Instance { get; }

    public Visibility DeleteButtonVisibility { get; set; }

    /// <summary>
    ///     Command for removing the pressed PlayerSelectionCard
    ///     -> Raises DeleteButtonPressed Event
    /// </summary>
    public static ICommand DeleteButtonCommand => new RelayCommand(instance =>
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
    ///     Color of the Player
    /// </summary>
    public Color PlayerColor
    {
        get => _playerColor;
        set => Update(ref _playerColor, value);
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

        Random rnd = new();
        _playerColor = Color.FromRgb((byte)rnd.Next(256), (byte)rnd.Next(256), (byte)rnd.Next(256));
    }

    public void UpdateDeleteButton()
    {
        OnPropertyChanged(nameof(DeleteButtonVisibility));
    }

    /// <summary>
    ///     Event Handler for the DeleteButton
    /// </summary>
    public static event EventHandler? DeleteButtonPressed;
}

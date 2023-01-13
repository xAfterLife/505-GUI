using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using _505_GUI_Battleships.Core;

namespace _505_GUI_Battleships.MVVM.Model;

public sealed class PlayerModel : ObservableObject
{
    private readonly string[] _playerImageList = { "../../../Resources/ProfilePictures/profilePic1.png", "../../../Resources/ProfilePictures/profilePic2.png", "../../../Resources/ProfilePictures/profilePic3.png", "../../../Resources/ProfilePictures/profilePic4.png", "../../../Resources/ProfilePictures/profilePic5.png", "../../../Resources/ProfilePictures/profilePic6.png", "../../../Resources/ProfilePictures/profilePic7.png", "../../../Resources/ProfilePictures/profilePic8.png", "../../../Resources/ProfilePictures/profilePic9.png" };
    private Visibility _deleteButtonVisibility;
    private uint _elo;
    private string _playerColor;
    private Guid _playerId;
    private string _playerImage;
    private string _playerName;
    private Canvas _visualPlayerBoard;

    public string PlayerImage
    {
        get => _playerImage;
        set => Update(ref _playerImage, value);
    }

    public Canvas VisualPlayerBoard
    {
        get => _visualPlayerBoard;
        set => Update(ref _visualPlayerBoard, value);
    }

    public ObservableCollection<ShipPlacementModel> Ships { get; set; } = new();

    /// <summary>
    ///     The Instance of PlayerModel used to pass as a Parameter to the static Command
    /// </summary>
    public PlayerModel Instance { get; }

    public Visibility DeleteButtonVisibility
    {
        get => _deleteButtonVisibility;
        set => Update(ref _deleteButtonVisibility, value);
    }

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

    public static ICommand SelectTargetPlayerCommand => new RelayCommand(instance =>
    {
        if ( instance is not PlayerModel player )
            return;

        SelectTargetPlayerCommandPressed?.Invoke(player, EventArgs.Empty);
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
    public string PlayerColor
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

        Random rInt = new();
        _playerImage = _playerImageList[rInt.Next(9)];
        Trace.WriteLine(rInt.Next(9));
        _playerColor = Color.FromRgb((byte)rInt.Next(256), (byte)rInt.Next(256), (byte)rInt.Next(256)).ToString();
    }

    /// <summary>
    ///     Event Handler for the DeleteButton
    /// </summary>
    public static event EventHandler? DeleteButtonPressed;

    /// <summary>
    ///     Event Handler for the SelectTargetPlayerCommand
    /// </summary>
    public static event EventHandler? SelectTargetPlayerCommandPressed;
}

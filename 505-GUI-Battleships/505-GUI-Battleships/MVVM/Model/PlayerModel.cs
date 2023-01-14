using System;
using System.Collections.ObjectModel;
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
    private string _playerColor;
    private string _playerImage;
    private string _playerName;
    private int _points;
    private Canvas? _visualPlayerBoard;
    private bool _winner;

    /// <summary>
    ///     Points of the Player in the current Game
    /// </summary>
    public int Points
    {
        get => _points;
        set => Update(ref _points, value);
    }

    /// <summary>
    ///     Image of the Player
    /// </summary>
    public string PlayerImage
    {
        get => _playerImage;
        set => Update(ref _playerImage, value);
    }

    /// <summary>
    ///     The Board that is Visual to all the Opponents
    /// </summary>
    public Canvas VisualPlayerBoard
    {
        get => _visualPlayerBoard!;
        set => Update(ref _visualPlayerBoard, value);
    }

    /// <summary>
    ///     List of the placed Ships by the Player
    /// </summary>
    public ObservableCollection<ShipPlacementModel> Ships { get; set; } = new();

    /// <summary>
    ///     The Instance of PlayerModel used to pass as a Parameter to the static Command
    /// </summary>
    public PlayerModel Instance { get; }

    /// <summary>
    ///     The Visibility if this Instance can be Removed from the PlayerList or not
    /// </summary>
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

    /// <summary>
    ///     Command that Selects a Player as Opponent and starts an Attack
    ///     -> Raises SelectTargetPlayerCommand Event
    /// </summary>
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
    ///     Is the Player Winning, son?
    /// </summary>
    public bool Winner
    {
        get => _winner;
        set
        {
            Update(ref _winner, value);
            OnPropertyChanged(nameof(CrownVisibility));
        }
    }

    /// <summary>
    ///     Visibility if the Winner's Crown is being displayed
    /// </summary>
    public Visibility CrownVisibility => Winner ? Visibility.Visible : Visibility.Hidden;

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
    public PlayerModel(string playerName = "Enter name")
    {
        Instance = this;
        _playerName = playerName;

        //Set Random PlayerImage & Color
        _playerImage = _playerImageList[Random.Shared.Next(9)];
        _playerColor = Color.FromRgb((byte)Random.Shared.Next(256), (byte)Random.Shared.Next(256), (byte)Random.Shared.Next(256)).ToString();
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

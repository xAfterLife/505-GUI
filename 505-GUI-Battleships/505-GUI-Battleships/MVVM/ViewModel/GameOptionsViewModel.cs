using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using _505_GUI_Battleships.Core;
using _505_GUI_Battleships.MVVM.Model;
using _505_GUI_Battleships.Services;

namespace _505_GUI_Battleships.MVVM.ViewModel;

internal sealed class GameOptionsViewModel : ObservableObject, IDisposable
{
    private int _boardHeight;
    private int _boardWith;
    private bool _firstOneOutCheck;
    private bool _lastManStandingCheck;
    private bool _playWithRoundsCheck;
    private int _roundCount;
    private Visibility _roundCountTextBlockVisibility;
    private Visibility _roundCountTextVisibility;

    /// <summary>
    ///     The Ships that every Player can set
    /// </summary>
    public ObservableCollection<ShipSizeSelectorModel> Ships { get; set; }

    /// <summary>
    ///     Get Back to the PlayerSelection
    /// </summary>
    public ICommand BackCommand => new RelayCommand(_ => ChangeViewModel.ChangeView(ChangeViewModel.ViewType.PlayerSelection, this));

    /// <summary>
    ///     Start the Game Command
    /// </summary>
    public static ICommand? StartGameCommand { get; set; }

    /// <summary>
    ///     Add a Ship Command
    /// </summary>
    public static ICommand? AddShipCommand { get; set; }

    /// <summary>
    ///     Remove a Ship Command
    /// </summary>
    public static ICommand? DeleteShipCommand { get; set; }

    /// <summary>
    ///     The Visibility of the AddShipCommand Button
    /// </summary>
    public Visibility AddShipCommandVisibility => Ships.Count >= 5 ? Visibility.Collapsed : Visibility.Visible;

    /// <summary>
    ///     The Visibility of the DeleteShipCommand Button
    /// </summary>
    public Visibility DeleteShipCommandVisibility => Ships.Count > 1 ? Visibility.Visible : Visibility.Hidden;

    /// <summary>
    ///     Toggles FirstOneOut off
    /// </summary>
    public static ICommand? LastManStandingCommand { get; set; }

    /// <summary>
    ///     Gets if LastManStanding is Checked
    /// </summary>
    public bool LastManStandingCheck
    {
        get => _lastManStandingCheck;
        set => Update(ref _lastManStandingCheck, value);
    }

    /// <summary>
    ///     Toggles LastManStanding off
    /// </summary>
    public static ICommand? FirstOneOutCommand { get; set; }

    /// <summary>
    ///     Gets if FirstOneOut is Checked
    /// </summary>
    public bool FirstOneOutCheck
    {
        get => _firstOneOutCheck;
        set => Update(ref _firstOneOutCheck, value);
    }

    /// <summary>
    ///     Check if PlayWithRounds is checked
    /// </summary>
    public bool PlayWithRoundsCheck
    {
        get => _playWithRoundsCheck;
        set => Update(ref _playWithRoundsCheck, value);
    }

    /// <summary>
    ///     Toggle on/off the Rounds-Menu
    /// </summary>
    public static ICommand? PlayWithRoundsCommand { get; set; }

    /// <summary>
    ///     The Visibility for one of the RoundCount Options
    /// </summary>
    public Visibility RoundCountTextBlockVisibility
    {
        get => _roundCountTextVisibility;
        set => Update(ref _roundCountTextBlockVisibility, value);
    }

    /// <summary>
    ///     The Visibility for one of the RoundCount Options
    /// </summary>
    public Visibility RoundCountTextBoxVisibility
    {
        get => _roundCountTextVisibility;
        set => Update(ref _roundCountTextVisibility, value);
    }

    /// <summary>
    ///     The Max-Rounds Double-Binding
    /// </summary>
    public int? RoundCount
    {
        get => _roundCount;
        set
        {
            if ( !value.HasValue )
                return;

            // Sanity Checking the Input
            switch ( value )
            {
                case > 50:
                    value = 50;
                    MessageBox.Show("Round Count needs to be between 15 and 50", "Information", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                    break;
                case < 15:
                    value = 15;
                    MessageBox.Show("Round Count needs to be between 15 and 50", "Information", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                    break;
            }

            Update(ref _roundCount, value.Value);
        }
    }

    /// <summary>
    ///     Width of the PlayerBoard Double-Binding
    /// </summary>
    public int BoardWidth
    {
        get => _boardWith;
        set
        {
            // Sanity Checking the Input
            switch ( value )
            {
                case > 15:
                    value = 15;
                    MessageBox.Show("BoardWidth and Height need to be between 7 and 15", "Information", MessageBoxButton.OK);
                    break;
                case < 7:
                    value = 7;
                    MessageBox.Show("BoardWidth and Height need to be between 7 and 15", "Information", MessageBoxButton.OK);
                    break;
            }

            Update(ref _boardWith, value);
        }
    }

    /// <summary>
    ///     Height of the PlayerBoard Double-Binding
    /// </summary>
    public int BoardHeight
    {
        get => _boardHeight;
        set
        {
            switch ( value )
            {
                // Sanity Checking the Input
                case > 15:
                    value = 15;
                    MessageBox.Show("BoardWidth and Height need to be between 7 and 15", "Information", MessageBoxButton.OK);
                    break;
                case < 7:
                    value = 7;
                    MessageBox.Show("BoardWidth and Height need to be between 7 and 15", "Information", MessageBoxButton.OK);
                    break;
            }

            Update(ref _boardHeight, value);
        }
    }

    /// <summary>
    ///     Constructor of the GameOptionsViewModel
    /// </summary>
    public GameOptionsViewModel()
    {
        LastManStandingCheck = true;
        FirstOneOutCheck = false;
        PlayWithRoundsCheck = true;

        //Default Initializers
        BoardWidth = 10;
        BoardHeight = 10;
        RoundCount = 15;

        var gameService = GameDataService.GetInstance();
        Ships = new ObservableCollection<ShipSizeSelectorModel> { new() };

        //Late Bind the Commands
        LastManStandingCommand = new RelayCommand(_ => FirstOneOutCheck = !LastManStandingCheck);
        FirstOneOutCommand = new RelayCommand(_ => LastManStandingCheck = !FirstOneOutCheck);
        PlayWithRoundsCommand = new RelayCommand(_ =>
        {
            switch ( PlayWithRoundsCheck )
            {
                case true:
                    RoundCountTextBlockVisibility = Visibility.Visible;
                    RoundCountTextBoxVisibility = Visibility.Visible;
                    RoundCount = 15; // Back to default
                    break;
                case false:
                    RoundCountTextBlockVisibility = Visibility.Hidden;
                    RoundCountTextBoxVisibility = Visibility.Hidden;
                    RoundCount = null;
                    break;
            }
        });
        AddShipCommand = new RelayCommand(_ =>
        {
            Ships.Add(new ShipSizeSelectorModel());

            //Call the OnPropertyChanged Function because those Fields don't Update themselves
            OnPropertyChanged(nameof(AddShipCommandVisibility));
            OnPropertyChanged(nameof(DeleteShipCommandVisibility));
        });

        DeleteShipCommand = new RelayCommand(_ =>
        {
            Ships.RemoveAt(Ships.Count - 1);

            //Call the OnPropertyChanged Function because those Fields don't Update themselves
            OnPropertyChanged(nameof(AddShipCommandVisibility));
            OnPropertyChanged(nameof(DeleteShipCommandVisibility));
        });

        StartGameCommand = new RelayCommand(_ =>
        {
            var shipList = Ships.Select(ship => ship.ShipImageListIndex + 1).ToList();
            var mode = FirstOneOutCheck ? GameMode.FirstOneOut : GameMode.LastManStanding;
            var rounds = PlayWithRoundsCheck ? RoundCount : null;
            // Create a List of the Ships we use
            var bitmapImages = new Collection<BitmapImage>
            {
                new(new Uri("pack://application:,,,/505-GUI-Battleships;component/Resources/Ships/1ShipPatrolHorizontal.png")),
                new(new Uri("pack://application:,,,/505-GUI-Battleships;component/Resources/Ships/1ShipPatrolVertical.png")),
                new(new Uri("pack://application:,,,/505-GUI-Battleships;component/Resources/Ships/2ShipRescueHorizontal.png")),
                new(new Uri("pack://application:,,,/505-GUI-Battleships;component/Resources/Ships/2ShipRescueVertical.png")),
                new(new Uri("pack://application:,,,/505-GUI-Battleships;component/Resources/Ships/3ShipSubMarineHorizontal.png")),
                new(new Uri("pack://application:,,,/505-GUI-Battleships;component/Resources/Ships/3ShipSubMarineVertical.png")),
                new(new Uri("pack://application:,,,/505-GUI-Battleships;component/Resources/Ships/4ShipDestroyerHorizontal.png")),
                new(new Uri("pack://application:,,,/505-GUI-Battleships;component/Resources/Ships/4ShipDestroyerVertical.png")),
                new(new Uri("pack://application:,,,/505-GUI-Battleships;component/Resources/Ships/5ShipBattleshipHorizontal.png")),
                new(new Uri("pack://application:,,,/505-GUI-Battleships;component/Resources/Ships/5ShipBattleshipVertical.png"))
            };

            gameService.Initialize(BoardHeight, BoardWidth, mode, rounds);

            // Fill the Game Options Shiplist via the set Ships in the Options Screen
            foreach ( var ship in shipList )
                switch ( ship )
                {
                    case 1:
                        gameService.ShipModels.Add(new ShipModel(1, bitmapImages[0], bitmapImages[1]));
                        break;

                    case 2:
                        gameService.ShipModels.Add(new ShipModel(2, bitmapImages[2], bitmapImages[3]));
                        break;

                    case 3:
                        gameService.ShipModels.Add(new ShipModel(3, bitmapImages[4], bitmapImages[5]));
                        break;

                    case 4:
                        gameService.ShipModels.Add(new ShipModel(4, bitmapImages[6], bitmapImages[7]));
                        break;

                    case 5:
                        gameService.ShipModels.Add(new ShipModel(5, bitmapImages[8], bitmapImages[9]));
                        break;
                }

            gameService.SetupPlayerShipModels();
            ChangeViewModel.ChangeView(ChangeViewModel.ViewType.ShipSelection, this);
        });
    }

    /// <summary>
    ///     Implement the IDisposable Interface
    /// </summary>
    public void Dispose() {}
}

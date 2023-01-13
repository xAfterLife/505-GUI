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

internal sealed class GameOptionsViewModel : ObservableObject
{
    private readonly GameDataService _gameService;

    private int _boardHeight;

    private int _boardWith;

    private bool _firstOneOutCheck;

    private bool _lastManStandingCheck;

    private bool _playWithRoundsCheck;

    private int _roundCount;

    private Visibility _roundCountTextBlockVisibility;

    private Visibility _roundCountTextVisibility;

    public ObservableCollection<ShipSizeSelectorModel> Ships { get; set; }

    public static ICommand BackCommand => new RelayCommand(_ => ChangeViewModel.ChangeView(ChangeViewModel.ViewType.PlayerSelection));
    public static ICommand? StartGameCommand { get; set; }
    public static ICommand? AddShipCommand { get; set; }
    public static ICommand? DeleteShipCommand { get; set; }
    public Visibility AddShipCommandVisibility => Ships.Count >= 5 ? Visibility.Collapsed : Visibility.Visible;
    public Visibility DeleteShipCommandVisibility => Ships.Count > 1 ? Visibility.Visible : Visibility.Hidden;

    public static ICommand? LastManStandingCommand { get; set; }

    public bool LastManStandingCheck
    {
        get => _lastManStandingCheck;
        set => Update(ref _lastManStandingCheck, value);
    }

    public static ICommand? FirstOneOutCommand { get; set; }

    public bool FirstOneOutCheck
    {
        get => _firstOneOutCheck;
        set => Update(ref _firstOneOutCheck, value);
    }

    public bool PlayWithRoundsCheck
    {
        get => _playWithRoundsCheck;
        set => Update(ref _playWithRoundsCheck, value);
    }

    public static ICommand? PlayWithRoundsCommand { get; set; }

    public Visibility RoundCountTextBlockVisibility
    {
        get => _roundCountTextVisibility;
        set => Update(ref _roundCountTextBlockVisibility, value);
    }

    public Visibility RoundCountTextBoxVisibility
    {
        get => _roundCountTextVisibility;
        set => Update(ref _roundCountTextVisibility, value);
    }

    public int? RoundCount
    {
        get => _roundCount;
        set
        {
            if ( !value.HasValue )
                return;

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

    public int BoardWidth
    {
        get => _boardWith;
        set
        {
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

    public int BoardHeight
    {
        get => _boardHeight;
        set
        {
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

            Update(ref _boardHeight, value);
        }
    }

    public GameOptionsViewModel()
    {
        LastManStandingCheck = true;
        FirstOneOutCheck = false;
        PlayWithRoundsCheck = true;

        // TODO: Set min/max width/height, rule for round count would be advisible
        BoardWidth = 10;
        BoardHeight = 10;
        RoundCount = 15;

        LastManStandingCommand = new RelayCommand(_ =>
        {
            FirstOneOutCheck = !LastManStandingCheck;
        });

        FirstOneOutCommand = new RelayCommand(_ =>
        {
            LastManStandingCheck = !FirstOneOutCheck;
        });

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

            OnPropertyChanged(nameof(RoundCountTextBlockVisibility));
            OnPropertyChanged(nameof(RoundCountTextBoxVisibility));
        });

        _gameService = GameDataService.GetInstance();
        var players = _gameService.PlayerModels;

        Ships = new ObservableCollection<ShipSizeSelectorModel> { new() };

        AddShipCommand = new RelayCommand(_ =>
        {
            Ships.Add(new ShipSizeSelectorModel());

            OnPropertyChanged(nameof(AddShipCommandVisibility));
            OnPropertyChanged(nameof(DeleteShipCommandVisibility));
        });

        DeleteShipCommand = new RelayCommand(_ =>
        {
            Ships.RemoveAt(Ships.Count - 1);

            OnPropertyChanged(nameof(AddShipCommandVisibility));
            OnPropertyChanged(nameof(DeleteShipCommandVisibility));
        });

        StartGameCommand = new RelayCommand(_ =>
        {
            GameMode mode = 0;
            int? rounds;

            if ( FirstOneOutCheck )
                mode = (GameMode)1;

            if ( PlayWithRoundsCheck )
                rounds = RoundCount;
            else
                rounds = null;

            _gameService.Initialize(BoardHeight, BoardWidth, mode, rounds);

            var shipList = Ships.Select(ship => ship.ShipImageListIndex + 1).ToList();
            var urisources = new Collection<Uri>
            {
                new("pack://application:,,,/505-GUI-Battleships;component/Ressources/Ships/1ShipPatrolHorizontal.png"),
                new("pack://application:,,,/505-GUI-Battleships;component/Ressources/Ships/1ShipPatrolVertical.png"),
                new("pack://application:,,,/505-GUI-Battleships;component/Ressources/Ships/2ShipRescueHorizontal.png"),
                new("pack://application:,,,/505-GUI-Battleships;component/Ressources/Ships/2ShipRescueVertical.png"),
                new("pack://application:,,,/505-GUI-Battleships;component/Ressources/Ships/3ShipSubMarineHorizontal.png"),
                new("pack://application:,,,/505-GUI-Battleships;component/Ressources/Ships/3ShipSubMarineVertical.png"),
                new("pack://application:,,,/505-GUI-Battleships;component/Ressources/Ships/4ShipDestroyerHorizontal.png"),
                new("pack://application:,,,/505-GUI-Battleships;component/Ressources/Ships/4ShipDestroyerVertical.png"),
                new("pack://application:,,,/505-GUI-Battleships;component/Ressources/Ships/5ShipBattleshipHorizontal.png"),
                new("pack://application:,,,/505-GUI-Battleships;component/Ressources/Ships/5ShipBattleshipVertical.png")
            };
            var bitmapImages = new Collection<BitmapImage>();
            foreach ( var uri in urisources )
                bitmapImages.Add(new BitmapImage(uri));

            foreach ( var ship in shipList )
                switch ( ship )
                {
                    case 1:
                        _gameService.ShipModels.Add(new ShipModel(1, bitmapImages[0], bitmapImages[1]));
                        break;

                    case 2:
                        _gameService.ShipModels.Add(new ShipModel(2, bitmapImages[2], bitmapImages[3]));
                        break;

                    case 3:
                        _gameService.ShipModels.Add(new ShipModel(3, bitmapImages[4], bitmapImages[5]));
                        break;

                    case 4:
                        _gameService.ShipModels.Add(new ShipModel(4, bitmapImages[6], bitmapImages[7]));
                        break;

                    case 5:
                        _gameService.ShipModels.Add(new ShipModel(5, bitmapImages[8], bitmapImages[9]));
                        break;
                }

            _gameService.SetupPlayerShipModels();

            ChangeViewModel.ChangeView(ChangeViewModel.ViewType.ShipSelection);
        });
    }
}

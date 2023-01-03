using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using _505_GUI_Battleships.Core;
using _505_GUI_Battleships.MVVM.Model;
using _505_GUI_Battleships.Services;

namespace _505_GUI_Battleships.MVVM.ViewModel;

internal sealed class GameOptionsViewModel : ObservableObject
{
    private readonly GameDataService _gameService;

    public GameOptionsModel GameOptions;

    public ObservableCollection<ShipSizeSelectorModel> Ships { get; set; }

    public static ICommand? BackCommand => new RelayCommand(_ => ChangeViewModel.ChangeView(ChangeViewModel.ViewType.PlayerSelection));

    public static ICommand? StartGameCommand { get; set; }

    public static ICommand? AddShipCommand { get; set; }
    public static ICommand? DeleteShipCommand { get; set; }
    public Visibility AddShipCommandVisibility => Ships.Count >= 5 ? Visibility.Hidden : Visibility.Visible;
    public Visibility DeleteShipCommandVisibility => Ships.Count > 1 ? Visibility.Visible : Visibility.Hidden;

    public static ICommand? LastManStandingCommand { get; set; }
    public bool LastManStandingCheck { get; set; }

    public static ICommand? FirstOneOutCommand { get; set; }
    public bool FirstOneOutCheck { get; set; }

    public bool PlayWithRoundsCheck { get; set; }
    public static ICommand? PlayWithRoundsCommand { get; set; }

    public Visibility RoundCountTextBlockVisibility { get; set; }
    public Visibility RoundCountTextBoxVisibility { get; set; }

    public int? RoundCount { get; set; }

    public int BoardWidth { get; set; }
    public int BoardHeight { get; set; }

    public GameOptionsViewModel()
    {
        LastManStandingCheck = true;
        FirstOneOutCheck = false;
        PlayWithRoundsCheck = true;
        
        // TODO: Set min/max width/height, rule for round count would be advisible
        BoardWidth = 10;
        BoardHeight = 10;
        RoundCount = 10;

        LastManStandingCommand = new RelayCommand(_ =>
        {
            if ( LastManStandingCheck )
                FirstOneOutCheck = false;

            if ( LastManStandingCheck == false )
                FirstOneOutCheck = true;
            OnPropertyChanged(nameof(LastManStandingCheck));
            OnPropertyChanged(nameof(FirstOneOutCheck));
        });

        FirstOneOutCommand = new RelayCommand(_ =>
        {
            LastManStandingCheck = FirstOneOutCheck switch
            {
                true  => false,
                false => true
            };

            OnPropertyChanged(nameof(LastManStandingCheck));
            OnPropertyChanged(nameof(FirstOneOutCheck));
        });

        PlayWithRoundsCommand = new RelayCommand(_ =>
        {
            switch ( PlayWithRoundsCheck )
            {
                case true:
                    RoundCountTextBlockVisibility = Visibility.Visible;
                    RoundCountTextBoxVisibility = Visibility.Visible;
                    RoundCount = 10;                                        // Back to default
                    break;
                case false:
                    RoundCountTextBlockVisibility = Visibility.Hidden;
                    RoundCountTextBoxVisibility = Visibility.Hidden;
                    RoundCount = null;
                    break;
            }

            OnPropertyChanged(nameof(RoundCountTextBlockVisibility));
            OnPropertyChanged(nameof(RoundCountTextBoxVisibility));
            OnPropertyChanged(nameof(RoundCount));
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
            // TEST
            foreach ( var player in players )
            {
                Trace.WriteLine(player.PlayerName);
                Trace.WriteLine(player.PlayerColor);
            }

            GameMode Mode = 0;
            int? Rounds;
            
            if (LastManStandingCheck) Mode = (GameMode)0;
            if (FirstOneOutCheck) Mode = (GameMode)1;

            if (PlayWithRoundsCheck) Rounds = RoundCount;
            else Rounds = null;
            
            List<int> ShipList = new();

            foreach(var ship in Ships)
            {
                ShipList.Add(ship.ShipImageListIndex + 1);
            }

            ShipList.Sort();

            GameOptions = new(BoardHeight, BoardWidth, Mode, Rounds, ShipList);
            _gameService.GameOptions = GameOptions;

            Trace.WriteLine("GameMode:");
            Trace.WriteLine(_gameService.GameOptions.GameMode);

            Trace.WriteLine("Game Rounds:");
            Trace.WriteLine(_gameService.GameOptions.Rounds);

            Trace.WriteLine("Board Dimensions:");
            Trace.WriteLine(_gameService.GameOptions.BoardWidth);
            Trace.WriteLine("x");
            Trace.WriteLine(_gameService.GameOptions.BoardHeight);

            Trace.WriteLine(_gameService.GameOptions.ShipLengthList);

        });
    }
}

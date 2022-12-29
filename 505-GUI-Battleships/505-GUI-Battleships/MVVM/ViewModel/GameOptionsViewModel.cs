using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using _505_GUI_Battleships.Core;
using _505_GUI_Battleships.MVVM.Model;
using _505_GUI_Battleships.Services;

namespace _505_GUI_Battleships.MVVM.ViewModel;

internal sealed class GameOptionsViewModel : ObservableObject
{
    private readonly GameDataService _gameService;
    public ObservableCollection<ShipSizeSelectorModel> Ships { get; set; }

    public static ICommand? BackCommand => new RelayCommand(_ => ChangeViewModel.ChangeView(ChangeViewModel.ViewType.PlayerSelection));

    public static ICommand? StartGameCommand { get; set; }

    public static ICommand? AddShipCommand { get; set; }
    public static ICommand? DeleteShipCommand { get; set; }
    public Visibility AddShipCommandVisibility => Ships.Count >= 5 ? Visibility.Hidden : Visibility.Visible;
    public Visibility DeleteShipCommandVisibility => Ships.Count > 1 ? Visibility.Visible : Visibility.Hidden;

    // TODO: Bind all the options
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
                    break;
                case false:
                    RoundCountTextBlockVisibility = Visibility.Hidden;
                    RoundCountTextBoxVisibility = Visibility.Hidden;
                    break;
            }

            OnPropertyChanged(nameof(RoundCountTextBlockVisibility));
            OnPropertyChanged(nameof(RoundCountTextBoxVisibility));
        });

        _gameService = GameDataService.GetInstance();

        var players = _gameService.PlayerModels;

        Ships = new ObservableCollection<ShipSizeSelectorModel> { new() };

        // TODO: Enlarge and Reduce increment/ decrement twice -> should only incr/decr once

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

            // Placeholder logging to be later replaced by passing game options the gameservice

            //_gameService.

            Trace.WriteLine("LMS: ");
            Trace.WriteLine(LastManStandingCheck);

            Trace.WriteLine("FOO:");
            Trace.WriteLine(FirstOneOutCheck);

            Trace.WriteLine("Play with Rounds:");
            Trace.WriteLine(PlayWithRoundsCheck);

            Trace.WriteLine("Round Count:");
            Trace.WriteLine(RoundCount);

            Trace.WriteLine("Board Dimensions:");
            Trace.WriteLine(BoardWidth);
            Trace.WriteLine("x");
            Trace.WriteLine(BoardHeight);
        });
    }
}

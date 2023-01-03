using System.Collections.ObjectModel;
using System.Linq;
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
            FirstOneOutCheck = !LastManStandingCheck;

            OnPropertyChanged(nameof(LastManStandingCheck));
            OnPropertyChanged(nameof(FirstOneOutCheck));
        });

        FirstOneOutCommand = new RelayCommand(_ =>
        {
            LastManStandingCheck = !FirstOneOutCheck;

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
                    RoundCount = 10; // Back to default
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
            GameMode mode = 0;
            int? rounds;

            // if (LastManStandingCheck) Mode = (GameMode)0;
            if ( FirstOneOutCheck )
                mode = (GameMode)1;

            if ( PlayWithRoundsCheck )
                rounds = RoundCount;
            else
                rounds = null;

            var shipList = Ships.Select(ship => ship.ShipImageListIndex + 1).ToList();
            _gameService.GameOptions = new GameOptionsModel(BoardHeight, BoardWidth, mode, rounds, shipList);
            ;

            ChangeViewModel.ChangeView(ChangeViewModel.ViewType.ShipSelection);
        });
    }
}

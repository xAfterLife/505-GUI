using _505_GUI_Battleships.Core;
using _505_GUI_Battleships.MVVM.Model;
using _505_GUI_Battleships.Services;
using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Controls;

namespace _505_GUI_Battleships.MVVM.ViewModel;

internal sealed class GameOptionsViewModel : ObservableObject
{
    public ObservableCollection<ShipSizeSelectorModel> Ships { get; set; }


    // TEST
    private GameDataService _gameService;

    public static ICommand? BackCommand => new RelayCommand(_ => ChangeViewModel.ChangeView(ChangeViewModel.ViewType.PlayerSelection));

    public static ICommand? StartGameCommand { get; set; }

    public static ICommand? AddShipCommand { get; set; }
    public static ICommand? DeleteShipCommand { get; set; }
    public Visibility AddShipCommandVisibility => Ships.Count >= 5 ? Visibility.Hidden : Visibility.Visible;
    public Visibility DeleteShipCommandVisibility => Ships.Count > 1 ? Visibility.Visible : Visibility.Hidden;


    // TODO: Bind all the options

    public CheckBox LastManStandingCheck { get; set; }
    public CheckBox FirstOneOutCheck { get; set; }
   
    public CheckBox PlayWithRoundsCheck { get; set; }
    
    public int? RoundCount { get; set; }

    public int BoardWidth { get; set; }
    public int BoardHeight { get; set; }

    public Visibility RoundCountVisibility => (bool)PlayWithRoundsCheck.IsChecked ? Visibility.Visible : Visibility.Hidden;




    public GameOptionsViewModel() 
    {

        // TODO: Make it so only one checkbox can be checked at a time

        /*if (LastManStandingCheck.IsChecked.Value == true) 
        { 
            FirstOneOutCheck.IsChecked = false; 
            OnPropertyChanged(nameof(FirstOneOutCheck));
            OnPropertyChanged(nameof(LastManStandingCheck));
        }
        if (FirstOneOutCheck.IsChecked.Value == true) 
        { 
            LastManStandingCheck.IsChecked = false;
            OnPropertyChanged(nameof(LastManStandingCheck));
            OnPropertyChanged(nameof(FirstOneOutCheck));
        }*/



        _gameService = GameDataService.GetInstance();

        ObservableCollection<PlayerModel> Players = _gameService.PlayerModels;

        Ships = new ObservableCollection<ShipSizeSelectorModel> { new(), new() };

        // TODO: Enlarge and Reduce increment/ decrement twice -> should only incr/decr once

        ShipSizeSelectorModel.EnlargeShipSizeCommandPressed += (sender, _) =>
        {
            if (sender is ShipSizeSelectorModel ShipSizeSelector)
                ShipSizeSelector.IncrementIndex();
        };
        
        ShipSizeSelectorModel.ReduceShipSizeCommandPressed += (sender, _) =>
        {
            if (sender is ShipSizeSelectorModel ShipSizeSelector)
                ShipSizeSelector.DecrementIndex();
        };



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
            foreach ( var player in Players )
            {
                Trace.WriteLine(player.PlayerName);
                Trace.WriteLine(player.PlayerColor);
            }


            // Placeholder logging to be later replaced by passing game options the gameservice

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

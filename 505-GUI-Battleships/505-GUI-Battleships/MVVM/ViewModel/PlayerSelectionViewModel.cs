using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using _505_GUI_Battleships.Core;
using _505_GUI_Battleships.MVVM.Model;
using _505_GUI_Battleships.Services;

namespace _505_GUI_Battleships.MVVM.ViewModel;

internal sealed class PlayerSelectionViewModel : ObservableObject, IDisposable
{
    /// <summary>
    ///     Collection of Players from which the PlayerSelectionCards are created
    /// </summary>
    public ObservableCollection<PlayerModel> Players { get; set; }

    /// <summary>
    ///     Binding for the AddPlayerButton Visibility property
    /// </summary>
    public Visibility AddPlayerButtonVisibility => Players.Count >= 4 ? Visibility.Collapsed : Visibility.Visible;

    /// <summary>
    ///     AddPlayer Command, adds new PlayerModel Object to list and enables deletebutton visibility if conditions are met
    /// </summary>
    public static ICommand? AddPlayerCommand { get; set; }

    /// <summary>
    ///     GoToGameOptionsCommand Command, changes current view to GameOptionsView and passes Players to the gameservice
    /// </summary>
    public static ICommand? GoToGameOptionsCommand { get; set; }

    /// <summary>
    ///     Backbutton Command
    /// </summary>
    public static ICommand? BackCommand { get; set; }

    /// <summary>
    ///     ctor
    /// </summary>
    public PlayerSelectionViewModel()
    {
        var gameService = GameDataService.GetInstance();

        //creates Players-List if no players exist, loads in players if they already exist
        if ( !gameService.PlayerModels.Any() )
            Players = new ObservableCollection<PlayerModel> { new(), new() };
        else
            Players = gameService.PlayerModels;

        if ( Players.Count <= 2 )
            foreach ( var playerModel in Players )
                playerModel.DeleteButtonVisibility = Visibility.Hidden;

        //Subscribe to DeleteButtonPressed and Remove the Players from the List
        PlayerModel.DeleteButtonPressed += OnPlayerModelOnDeleteButtonPressed!;

        AddPlayerCommand = new RelayCommand(_ =>
        {
            Players.Add(new PlayerModel());

            if ( Players.Count > 2 )
                foreach ( var playerModel in Players )
                    playerModel.DeleteButtonVisibility = Visibility.Visible;

            OnPropertyChanged(nameof(AddPlayerButtonVisibility));
        });

        GoToGameOptionsCommand = new RelayCommand(_ =>
        {
            gameService.PlayerModels = Players;
            ChangeViewModel.ChangeView(ChangeViewModel.ViewType.GameOptions, this);
        });

        BackCommand = new RelayCommand(_ =>
        {
            gameService.PlayerModels.Clear();
            ChangeViewModel.ChangeView(ChangeViewModel.ViewType.Start, this);
        });
    }

    /// <summary>
    /// Dispose current view model and unsubscribe OnPlayerModelOnDeleteButtonPressed
    /// </summary>
    public void Dispose()
    {
        PlayerModel.DeleteButtonPressed -= OnPlayerModelOnDeleteButtonPressed!;
    }

    /// <summary>
    /// removes player from Players list and turns visibility of delete button
    /// if only 2 players are in said list
    /// </summary>
    private void OnPlayerModelOnDeleteButtonPressed(object sender, EventArgs _)
    {
        if ( sender is PlayerModel player )
            Players.Remove(player);

        if ( Players.Count <= 2 )
            foreach ( var playerModel in Players )
                playerModel.DeleteButtonVisibility = Visibility.Hidden;

        OnPropertyChanged(nameof(AddPlayerButtonVisibility));
    }
}

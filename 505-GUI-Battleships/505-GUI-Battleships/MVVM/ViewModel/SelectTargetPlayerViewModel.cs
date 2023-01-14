using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
using _505_GUI_Battleships.Core;
using _505_GUI_Battleships.MVVM.Model;
using _505_GUI_Battleships.Services;

namespace _505_GUI_Battleships.MVVM.ViewModel;

internal sealed class SelectTargetPlayerViewModel : ObservableObject, IDisposable
{
    private readonly GameDataService _gameService;
    private PlayerModel _currentPlayer;
    private string _roundCountText = string.Empty;
    private string _selectTargetPlayerHeading = string.Empty;

    /// <summary>
    /// Binding for the Heading
    /// </summary>
    public string SelectTargetPlayerHeading
    {
        get => _selectTargetPlayerHeading;
        set => Update(ref _selectTargetPlayerHeading, value);
    }

    public ObservableCollection<PlayerModel> TargetablePlayers { get; set; }

    /// <summary>
    /// Binding for the PlayerCard that shows the currently playing player
    /// </summary>
    public PlayerModel CurrentPlayer
    {
        get => _currentPlayer;
        set => Update(ref _currentPlayer, value);
    }

    /// <summary>
    /// Binding for the displayed RoundCount
    /// </summary>
    public string RoundCountText
    {
        get => _roundCountText;
        set => Update(ref _roundCountText, value);
    }

    /// <summary>
    /// ctor
    /// </summary>
    public SelectTargetPlayerViewModel()
    {
        _gameService = GameDataService.GetInstance();
        _currentPlayer = _gameService.CurrentPlayer!;
        TargetablePlayers = new ObservableCollection<PlayerModel>(_gameService.PlayerModels.Where(x => x != _currentPlayer));

        SelectTargetPlayerHeading = $"It's your turn to attack, {_currentPlayer.PlayerName}!";
        RoundCountText = _gameService.CurrentRound.ToString();

        //Subscribe to electTargetPlayerPressed
        PlayerModel.SelectTargetPlayerCommandPressed += SelectTargetPlayerPressed;
    }
    /// <summary>
    /// Dispose current view model and unsubscribe SelectTargetPlayerPressed
    /// </summary>
    public void Dispose()
    {
        PlayerModel.SelectTargetPlayerCommandPressed -= SelectTargetPlayerPressed;
    }

    /// <summary>
    /// select CurrentTarget and change into BoardAttackView
    /// </summary>
    private void SelectTargetPlayerPressed(object? sender, EventArgs args)
    {
        if ( sender is PlayerModel player )
            _gameService.CurrentTarget = player;
        ChangeViewModel.ChangeView(ChangeViewModel.ViewType.BoardAttack, this);
    }
}

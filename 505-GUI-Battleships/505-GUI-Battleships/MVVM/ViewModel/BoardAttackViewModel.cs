using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using _505_GUI_Battleships.Core;
using _505_GUI_Battleships.MVVM.Model;
using _505_GUI_Battleships.Services;

namespace _505_GUI_Battleships.MVVM.ViewModel;

internal sealed class BoardAttackViewModel : ObservableObject, IDisposable
{
    private readonly (int Width, int Height) _boardDimensions;
    private readonly GameDataService _gameService;
    private PlayerModel _attackerPlayerCard;
    private Grid _boardContainer;
    private bool _isInputEnabled = true;
    private Canvas _playerBoard;
    private PlayerModel _targetedPlayerCard;

    /// <summary>
    ///     Current Round Binding
    /// </summary>
    public string Round => $"Round {_gameService.CurrentRound}";

    /// <summary>
    ///     Visibility for the Back Button to prevent players from going into the Target Selection when only 2 Players are left
    /// </summary>
    public Visibility BackButtonVisibility => _gameService.PlayerModels.Count > 2 ? Visibility.Visible : Visibility.Hidden;

    /// <summary>
    ///     Disable Input during the Rocket Animation
    /// </summary>
    public bool IsInputEnabled
    {
        get => _isInputEnabled;
        set => Update(ref _isInputEnabled, value);
    }

    /// <summary>
    ///     Container for Board and Axis-Descriptors
    /// </summary>
    public Grid BoardContainer
    {
        get => _boardContainer;
        set => Update(ref _boardContainer, value);
    }

    /// <summary>
    ///     The Board of the Target player to Attack on
    /// </summary>
    public Canvas PlayerBoard
    {
        get => _playerBoard;
        set => Update(ref _playerBoard, value);
    }

    /// <summary>
    ///     Display your Target, to see the fear in their eyes
    /// </summary>
    public PlayerModel TargetedPlayerCard
    {
        get => _targetedPlayerCard;
        set => Update(ref _targetedPlayerCard, value);
    }

    /// <summary>
    ///     Display your own Playercard
    /// </summary>
    public PlayerModel AttackerPlayerCard
    {
        get => _attackerPlayerCard;
        set => Update(ref _attackerPlayerCard, value);
    }

    /// <summary>
    /// </summary>
    public ICommand? SelectPlayerViewCommand { get; }

    /// <summary>
    ///     Constructor for BoardAttackViewModel
    /// </summary>
    public BoardAttackViewModel()
    {
        _gameService = GameDataService.GetInstance();
        TargetedPlayerCard = _gameService.CurrentTarget!;
        AttackerPlayerCard = _gameService.CurrentPlayer!;
        _boardDimensions = (_gameService.GameBoard!.Width, _gameService.GameBoard!.Height);

        //Late Binding because we need the Current Instance
        SelectPlayerViewCommand = new RelayCommand(_ => ChangeViewModel.ChangeView(ChangeViewModel.ViewType.SelectTargetPlayer, this));
        SetupBoardContainer();
    }

    /// <summary>
    ///     Dispose the Instance and remove the EventListener
    /// </summary>
    public void Dispose()
    {
        PlayerBoard.MouseLeftButtonDown -= MouseLeftButtonDownHandler;
    }

    /// <summary>
    ///     Initialize the Board
    /// </summary>
    private void SetupBoardContainer()
    {
        //Create the Board which is a Grid and Add Definitions
        BoardContainer = new Grid();
        BoardContainer.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
        BoardContainer.RowDefinitions.Add(new RowDefinition { Height = new GridLength(_boardDimensions.Height, GridUnitType.Star) });
        BoardContainer.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
        BoardContainer.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(_boardDimensions.Width, GridUnitType.Star) });

        Canvas xAxis = new() { Width = _boardDimensions.Width, Height = 1, LayoutTransform = new ScaleTransform(1, -1) };
        Canvas yAxis = new() { Width = 1, Height = _boardDimensions.Height, LayoutTransform = new ScaleTransform(1, -1) };

        //Fill the X-Asis Descriptors
        for ( var i = 0; i < _boardDimensions.Width; i++ )
        {
            var textBlock = new TextBlock
            {
                Text = ((char)('A' + i)).ToString(),
                FontSize = 0.5,
                Foreground = new SolidColorBrush(Colors.LightGreen),
                LayoutTransform = new ScaleTransform(1, -1),
                TextAlignment = TextAlignment.Center,
                VerticalAlignment = VerticalAlignment.Stretch,
                Width = 1,
                Height = 1
            };

            xAxis.Children.Add(textBlock);
            Canvas.SetLeft(xAxis.Children[i], i);
        }

        Grid.SetRow(xAxis, 0);
        Grid.SetColumn(xAxis, 1);

        //Fill the Y-Asis Descriptors
        for ( var i = 0; i < _boardDimensions.Height; i++ )
        {
            var textBlock = new TextBlock
            {
                Text = (i + 1).ToString(),
                FontSize = 0.5,
                Foreground = new SolidColorBrush(Colors.White),
                LayoutTransform = new ScaleTransform(1, -1),
                TextAlignment = TextAlignment.Center,
                VerticalAlignment = VerticalAlignment.Stretch,
                Width = 1,
                Height = 1
            };
            yAxis.Children.Add(textBlock);
            Canvas.SetTop(yAxis.Children[i], i - 0.2);
        }

        Grid.SetRow(yAxis, 1);
        Grid.SetColumn(yAxis, 0);

        //Add the Legend/Descriptors
        BoardContainer.Children.Add(xAxis);
        BoardContainer.Children.Add(yAxis);

        PlayerBoard = _gameService.CurrentTarget!.VisualPlayerBoard;
        PlayerBoard.ClipToBounds = false;

        //Add an EventListener to get the Targeted Cell
        PlayerBoard.MouseLeftButtonDown += MouseLeftButtonDownHandler;

        Grid.SetRow(_playerBoard, 1);
        Grid.SetColumn(_playerBoard, 1);

        ((Panel)PlayerBoard.Parent)?.Children.Remove(PlayerBoard);
        BoardContainer.Children.Add(PlayerBoard);
    }

    /// <summary>
    ///     Handle attack on a Cell
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void MouseLeftButtonDownHandler(object sender, MouseButtonEventArgs e)
    {
        var isShipHit = false;
        ShipPlacementModel struckShip = null!;

        //Get the Click-Position and fix it to the Tile
        var clickPosition = e.GetPosition((IInputElement)sender);
        clickPosition.X = Math.Round(clickPosition.X - 0.5);
        if ( clickPosition.X < 0 )
            clickPosition.X = 0;
        clickPosition.Y = Math.Round(clickPosition.Y - 0.5);
        if ( clickPosition.Y > _boardDimensions.Height - 1 )
            clickPosition.Y = _boardDimensions.Height - 1;

        //If the Field has already been clicked on
        if ( _playerBoard.Children.OfType<Image>().Any(hit => (clickPosition.X == Canvas.GetLeft(hit)) & (clickPosition.Y == Canvas.GetTop(hit))) )
            return;

        var ship = _gameService.CurrentTarget!.Ships.FirstOrDefault(x => x.IsShipHit(clickPosition));

        if ( ship != null )
        {
            isShipHit = true;
            struckShip = ship;
        }

        StartAnimation(clickPosition, isShipHit, struckShip!);
    }

    /// <summary>
    ///     Start the Animation of the Rocket
    /// </summary>
    /// <param name="clickPosition"></param>
    /// <param name="isShipHit"></param>
    /// <param name="struckShip"></param>
    private void StartAnimation(Point clickPosition, bool isShipHit, ShipPlacementModel struckShip)
    {
        var imageSource = new BitmapImage(new Uri("pack://application:,,,/505-GUI-Battleships;component/Resources/RocketStraight.png", UriKind.RelativeOrAbsolute));

        var rocket = new Image { Width = 1, Height = 2, Source = imageSource };
        var tt = new TranslateTransform();
        rocket.RenderTransform = tt;
        _playerBoard.Children.Add(rocket);

        var storyboard = new Storyboard();

        var rocketAnimationX = new DoubleAnimation { From = clickPosition.X, To = clickPosition.X, Duration = new Duration(TimeSpan.FromSeconds(2)) };
        var rocketAnimationY = new DoubleAnimation { From = clickPosition.Y + 15, To = clickPosition.Y, Duration = new Duration(TimeSpan.FromSeconds(2)) };

        // Create a Storyboard.TargetName and Storyboard.TargetProperty
        Storyboard.SetTarget(rocketAnimationX, rocket);
        Storyboard.SetTargetProperty(rocketAnimationX, new PropertyPath("(UIElement.RenderTransform).(TranslateTransform.X)"));
        Storyboard.SetTarget(rocketAnimationY, rocket);
        Storyboard.SetTargetProperty(rocketAnimationY, new PropertyPath("(UIElement.RenderTransform).(TranslateTransform.Y)"));

        storyboard.Children.Add(rocketAnimationX);
        storyboard.Children.Add(rocketAnimationY);

        // Action that Fires once the Animation has ended
        async void ActionDelegate(object? o, EventArgs e)
        {
            rocket.RenderTransform = new TranslateTransform();
            Canvas.SetLeft(rocket, clickPosition.X);
            Canvas.SetTop(rocket, clickPosition.Y);

            if ( !isShipHit )
            {
                // Indicate the miss by a Blue Circle and a Water-Sound
                SoundPlayerService.PlaySound(SoundPlayerService.SoundType.Wassertreffer);
                rocket.Source = new BitmapImage(new Uri("pack://application:,,,/505-GUI-Battleships;component/Resources/RingBlue.png", UriKind.RelativeOrAbsolute));
                rocket.Height = 1;

                //Wait for the Sound and Animation to play and the User to see the Result of his Decision
                await Task.Delay(1500);
                _gameService.SetNextPlayer();

                // Game has ended?
                if ( _gameService.CurrentRound >= _gameService.GameOptions!.Rounds )
                {
                    ChangeViewModel.ChangeView(ChangeViewModel.ViewType.EndOfGame, this);
                    return;
                }

                // Throw next Player into StarterScreen or Directly into the next Attack
                ((Panel)PlayerBoard.Parent)?.Children.Remove(PlayerBoard);

                if ( _gameService.PlayerModels.Count == 2 )
                {
                    _gameService.CurrentTarget = _gameService.PlayerModels.First(x => x != _gameService.CurrentTarget);
                    ChangeViewModel.ChangeView(ChangeViewModel.ViewType.BoardAttack, this);
                }
                else
                {
                    ChangeViewModel.ChangeView(ChangeViewModel.ViewType.SelectTargetPlayer, this);
                }
            }
            else
            {
                SoundPlayerService.PlaySound(SoundPlayerService.SoundType.Treffer);

                _gameService.CurrentPlayer!.Points += 6 - struckShip.Length;
                rocket.Source = new BitmapImage(new Uri("pack://application:,,,/505-GUI-Battleships;component/Resources/XRed.png", UriKind.RelativeOrAbsolute));
                rocket.Height = 1;

                // Detect if a Ship has sunk
                var finalHit = struckShip.GetPoisitionList().All(position => _playerBoard.Children.Cast<UIElement>().Any(hit => position == new Point(Canvas.GetLeft(hit), Canvas.GetTop(hit))));

                if ( finalHit )
                {
                    await Task.Delay(1500);
                    SoundPlayerService.PlaySound(SoundPlayerService.SoundType.FinalTreffer);
                }

                await Task.Delay(1500);
                Trace.WriteLine(finalHit);

                // Detect if all Ships from this player sunk
                var allShipsDestroyed = _gameService.CurrentTarget!.Ships.Select(x => x.GetPoisitionList()).All(ships => ships.All(position => _playerBoard.Children.Cast<UIElement>().Any(hit => position == new Point(Canvas.GetLeft(hit), Canvas.GetTop(hit)))));

                if ( allShipsDestroyed )
                {
                    _gameService.PlayerKnockOut(this);

                    // End the Game
                    if ( _gameService.CheckGameOver() )
                    {
                        _gameService.CurrentPlayer.Winner = true;
                        await Task.Delay(500);
                        SoundPlayerService.PlaySound(SoundPlayerService.SoundType.EnemyDestroyed);
                        ChangeViewModel.ChangeView(ChangeViewModel.ViewType.EndOfGame, this);
                    }
                    // Same CurrentUser next Attack
                    else
                    {
                        if ( _gameService.PlayerModels.Count == 2 )
                        {
                            _gameService.CurrentTarget = _gameService.PlayerModels.First(x => x != _gameService.CurrentPlayer);
                            ChangeViewModel.ChangeView(ChangeViewModel.ViewType.BoardAttack, this);
                        }
                        else
                        {
                            ChangeViewModel.ChangeView(ChangeViewModel.ViewType.SelectTargetPlayer, this);
                        }
                    }
                }
            }

            IsInputEnabled = true;
        }

        storyboard.Completed += ActionDelegate;
        IsInputEnabled = false;
        storyboard.Begin();
        SoundPlayerService.PlaySound(SoundPlayerService.SoundType.Geschoss);
    }
}

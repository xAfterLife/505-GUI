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

    public bool IsInputEnabled
    {
        get => _isInputEnabled;
        set => Update(ref _isInputEnabled, value);
    }

    public Grid BoardContainer
    {
        get => _boardContainer;
        set => Update(ref _boardContainer, value);
    }

    public Canvas PlayerBoard
    {
        get => _playerBoard;
        set => Update(ref _playerBoard, value);
    }

    public PlayerModel TargetedPlayerCard
    {
        get => _targetedPlayerCard;
        set => Update(ref _targetedPlayerCard, value);
    }

    public PlayerModel AttackerPlayerCard
    {
        get => _attackerPlayerCard;
        set => Update(ref _attackerPlayerCard, value);
    }

    public ICommand? BackToSelectPlayerTargetView { get; }

    public BoardAttackViewModel()
    {
        _gameService = GameDataService.GetInstance();
        _targetedPlayerCard = _gameService.CurrentTarget!;
        _attackerPlayerCard = _gameService.CurrentPlayer!;
        _boardDimensions = (_gameService.GameBoard!.Width, _gameService.GameBoard!.Height);

        BackToSelectPlayerTargetView = new RelayCommand(_ =>
        {
            ChangeViewModel.ChangeView(ChangeViewModel.ViewType.SelectTargetPlayer, this);
        });

        SetupBoardContainer();
    }

    public void Dispose()
    {
        PlayerBoard.MouseLeftButtonDown -= MouseLeftButtonDownHandler;
    }

    private void SetupBoardContainer()
    {
        BoardContainer = new Grid();
        BoardContainer.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
        BoardContainer.RowDefinitions.Add(new RowDefinition { Height = new GridLength(_boardDimensions.Height, GridUnitType.Star) });
        BoardContainer.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
        BoardContainer.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(_boardDimensions.Width, GridUnitType.Star) });

        Canvas xAxis = new() { Width = _boardDimensions.Width, Height = 1, LayoutTransform = new ScaleTransform(1, -1) };
        Canvas yAxis = new() { Width = 1, Height = _boardDimensions.Height, LayoutTransform = new ScaleTransform(1, -1) };

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

        BoardContainer.Children.Add(xAxis);
        BoardContainer.Children.Add(yAxis);

        PlayerBoard = _gameService.CurrentTarget!.VisualPlayerBoard;
        PlayerBoard.ClipToBounds = false;

        PlayerBoard.MouseLeftButtonDown += MouseLeftButtonDownHandler;

        Grid.SetRow(_playerBoard, 1);
        Grid.SetColumn(_playerBoard, 1);

        ((Panel)PlayerBoard.Parent)?.Children.Remove(PlayerBoard);
        BoardContainer.Children.Add(PlayerBoard);
    }

    private void MouseLeftButtonDownHandler(object sender, MouseButtonEventArgs e)
    {
        PlayerBoard.MouseLeftButtonDown -= MouseLeftButtonDownHandler;

        var clickPosition = e.GetPosition((IInputElement)sender);
        clickPosition.X = Math.Round(clickPosition.X - 0.5);
        if ( clickPosition.X < 0 )
            clickPosition.X = 0;
        clickPosition.Y = Math.Round(clickPosition.Y - 0.5);
        if ( clickPosition.Y > _boardDimensions.Height - 1 )
            clickPosition.Y = _boardDimensions.Height - 1;
        Trace.WriteLine(clickPosition.ToString());
        var isShipHit = false;
        ShipPlacementModel struckShip = null!;

        if ( _playerBoard.Children.OfType<Image>().Any(hit => (clickPosition.X == Canvas.GetLeft(hit)) & (clickPosition.Y == Canvas.GetTop(hit))) )
            return;

        var ship = _gameService.CurrentTarget!.Ships.FirstOrDefault(x => x.IsShipHit(clickPosition));

        if ( ship != null )
        {
            isShipHit = true;
            struckShip = ship;
        }

        TestAnimation(clickPosition, isShipHit, struckShip!);
    }

    private void TestAnimation(Point clickPosition, bool isShipHit, ShipPlacementModel struckShip)
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

        // Start the storyboard
        async void ActionDelegate(object? o, EventArgs e)
        {
            rocket.RenderTransform = new TranslateTransform();
            Canvas.SetLeft(rocket, clickPosition.X);
            Canvas.SetTop(rocket, clickPosition.Y);
            if ( !isShipHit )
            {
                SoundPlayerService.PlaySound(SoundPlayerService.SoundType.Wassertreffer);
                rocket.Source = new BitmapImage(new Uri("pack://application:,,,/505-GUI-Battleships;component/Resources/RingBlue.png", UriKind.RelativeOrAbsolute));
                rocket.Height = 1;

                await Task.Delay(1500);

                _gameService.SetNextPlayer();

                if ( _gameService.CurrentRound >= _gameService.GameOptions!.Rounds )
                {
                    ChangeViewModel.ChangeView(ChangeViewModel.ViewType.Start, this);
                    return;
                }

                ((Panel)PlayerBoard.Parent)?.Children.Remove(PlayerBoard);
                ChangeViewModel.ChangeView(ChangeViewModel.ViewType.SelectTargetPlayer, this);
            }
            else
            {
                //TODO: ADD PlayerScore
                _gameService.CurrentPlayer!.Points += 6 - struckShip.Length;
                rocket.Source = new BitmapImage(new Uri("pack://application:,,,/505-GUI-Battleships;component/Resources/RingRed.png", UriKind.RelativeOrAbsolute));
                rocket.Height = 1;

                var finalHit = struckShip.GetPoisitionList().All(position => _playerBoard.Children.Cast<UIElement>().Any(hit => position == new Point(Canvas.GetLeft(hit), Canvas.GetTop(hit))));

                if ( finalHit )
                    //Schiff versenkt
                    SoundPlayerService.PlaySound(SoundPlayerService.SoundType.FinalTreffer);
                else
                    //Schiff getroffen
                    SoundPlayerService.PlaySound(SoundPlayerService.SoundType.Treffer);

                await Task.Delay(1500);
                Trace.WriteLine(finalHit);

                //TODO: Check if all Ships are Destroyed
                var allShipsDestroyed = _gameService.CurrentTarget!.Ships.Select(x => x.GetPoisitionList()).All(ships => ships.All(position => _playerBoard.Children.Cast<UIElement>().Any(hit => position == new Point(Canvas.GetLeft(hit), Canvas.GetTop(hit)))));
                if ( allShipsDestroyed )
                    _gameService.PlayerKnockOut(this);
            }

            IsInputEnabled = true;
        }

        storyboard.Completed += ActionDelegate;
        IsInputEnabled = false;
        storyboard.Begin();
        SoundPlayerService.PlaySound(SoundPlayerService.SoundType.Geschoss);
    }
}

using _505_GUI_Battleships.Core;
using _505_GUI_Battleships.MVVM.Model;
using _505_GUI_Battleships.Services;
using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;

namespace _505_GUI_Battleships.MVVM.ViewModel;

internal sealed class BoardAttackViewModel: ObservableObject
{
    private PlayerModel _targetedPlayerCard;
    private PlayerModel _attackerPlayerCard;
    private Canvas _playerBoard;
    private Grid _boardContainer;
    private readonly (int Width, int Height) _boardDimensions;
    private readonly GameDataService _gameService;
    private bool _isInputEnabled = true;

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

    public static ICommand? BackToSelectPlayerTargetView { get; set; }



    /*public void SetupBoardAttack(PlayerModel targetedPlayerCard, PlayerModel attackerPlayerCard)
    {
        TargetedPlayerCard = targetedPlayerCard;
        AttackerPlayerCard = attackerPlayerCard;
        PlayerBoard = playerBoard;
    }*/


    public BoardAttackViewModel()
    {
        _gameService = GameDataService.GetInstance();
        _targetedPlayerCard = _gameService.CurrentTarget;
        _attackerPlayerCard = _gameService.CurrentPlayer;
        _boardDimensions = (_gameService.GameBoard!.Width, _gameService.GameBoard!.Height);
        SetupBoardContainer();
        BackToSelectPlayerTargetView = new RelayCommand(_ => {
            BoardContainer.Children.Remove(_playerBoard);
            ChangeViewModel.ChangeView(ChangeViewModel.ViewType.SelectTargetPlayer);
        });
        //TestAufrufe();
    }

    /*private void TestAufrufe()
    {
        SetupBoardAttack(_gameService.CurrentTarget, _gameService.CurrentPlayer);
    }*/

    private void SetupBoardContainer()
    {

        BoardContainer = new Grid();
        BoardContainer.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
        BoardContainer.RowDefinitions.Add(new RowDefinition { Height = new GridLength(_boardDimensions.Height, GridUnitType.Star) });
        BoardContainer.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
        BoardContainer.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(_boardDimensions.Width, GridUnitType.Star) });

        Canvas xAxis = new() { Width = _boardDimensions.Width, Height = 1, LayoutTransform = new ScaleTransform(1, -1) };
        Canvas yAxis = new() { Width = 1, Height = _boardDimensions.Height, LayoutTransform = new ScaleTransform(1, -1) };

        for (var i = 0; i < _boardDimensions.Width; i++)
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

        for (var i = 0; i < _boardDimensions.Height; i++)
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

        PlayerBoard = _gameService.CurrentTarget.VisualPlayerBoard;
        PlayerBoard.MouseLeftButtonDown += MouseLeftButtonDownHandler;
        PlayerBoard.ClipToBounds = false;
        Grid.SetRow(_playerBoard, 1);
        Grid.SetColumn(_playerBoard, 1);

        if (PlayerBoard.Parent != null)
        {
            ((Panel)PlayerBoard.Parent).Children.Remove(PlayerBoard);
            //PlayerBoard.MouseLeftButtonDown -= MouseLeftButtonDownHandler;
        }

        BoardContainer.Children.Add(PlayerBoard);
    }

    private void MouseLeftButtonDownHandler(object sender, MouseButtonEventArgs e)
    {
        var clickPosition = e.GetPosition((IInputElement)sender);
        clickPosition.X = Math.Round(clickPosition.X - 0.5);
        if ( clickPosition.X < 0)
            clickPosition.X = 0;
        clickPosition.Y = Math.Round(clickPosition.Y - 0.5);
        if (clickPosition.Y > _boardDimensions.Height - 1)
            clickPosition.Y = _boardDimensions.Height - 1;
        Trace.WriteLine(clickPosition.ToString());
        bool isShipHit = false;
        foreach (Image hit in _playerBoard.Children.OfType<Image>())
        {
            if (clickPosition.X == Canvas.GetLeft(hit) & clickPosition.Y == Canvas.GetTop(hit))
                return;
        }
        foreach (var x in _gameService.CurrentTarget.Ships)
        {
            if (x.IsShipHit(clickPosition))
            {
                isShipHit = true;
                break;
            }
        }
        TestAnimation(clickPosition, isShipHit);
    }

    private void TestAnimation(Point clickPosition, bool isShipHit)
    {

        BitmapImage imageSource = new BitmapImage(new Uri("pack://application:,,,/505-GUI-Battleships;component/Ressources/Rocket.png", UriKind.RelativeOrAbsolute));

        Image rocket = new Image()
        {
            Width = 1,
            Height = 1, 
            Source = imageSource
        };

        TranslateTransform tt = new TranslateTransform();
        rocket.RenderTransform = tt;
        //RotateTransform rotateTransform = new RotateTransform();
        //rect1.RenderTransform = rotateTransform;
        //rotateTransform.Angle = 180;
        int index = _playerBoard.Children.Add(rocket);
        //Image rect = (Image)_playerBoard.Children[index];
        Storyboard storyboard = new Storyboard();

        // Create a DoubleAnimation to animate the rectangle's Left property
        var doubleAnimationX1 = new DoubleAnimation();
        doubleAnimationX1.From = -4;
        doubleAnimationX1.To = (clickPosition.X + 3) / 2 - 3;
        doubleAnimationX1.Duration = new Duration(TimeSpan.FromSeconds(1));

        var doubleAnimationX2 = new DoubleAnimation();
        doubleAnimationX2.From = (clickPosition.X + 3) / 2 - 3;
        doubleAnimationX2.To = clickPosition.X;
        doubleAnimationX2.BeginTime = doubleAnimationX1.Duration.TimeSpan;
        doubleAnimationX2.Duration = new Duration(TimeSpan.FromSeconds(1));

        // Create a second DoubleAnimation to animate the rectangle's Top property
        var doubleAnimationY1 = new DoubleAnimation();
        doubleAnimationY1.From = 1;
        doubleAnimationY1.To = clickPosition.Y + 15;
        doubleAnimationY1.Duration = new Duration(TimeSpan.FromSeconds(1));

        var doubleAnimationY2 = new DoubleAnimation();
        doubleAnimationY2.From = clickPosition.Y + 15;
        doubleAnimationY2.To = clickPosition.Y;
        doubleAnimationY2.BeginTime = doubleAnimationY1.Duration.TimeSpan;
        doubleAnimationY2.Duration = new Duration(TimeSpan.FromSeconds(1));

        // Create a Storyboard.TargetName and Storyboard.TargetProperty
        Storyboard.SetTarget(doubleAnimationX1, rocket);
        Storyboard.SetTargetProperty(doubleAnimationX1, new PropertyPath("(UIElement.RenderTransform).(TranslateTransform.X)"));
        Storyboard.SetTarget(doubleAnimationY1, rocket);
        Storyboard.SetTargetProperty(doubleAnimationY1, new PropertyPath("(UIElement.RenderTransform).(TranslateTransform.Y)"));

        Storyboard.SetTarget(doubleAnimationX2, rocket);
        Storyboard.SetTargetProperty(doubleAnimationX2, new PropertyPath("(UIElement.RenderTransform).(TranslateTransform.X)"));
        Storyboard.SetTarget(doubleAnimationY2, rocket);
        Storyboard.SetTargetProperty(doubleAnimationY2, new PropertyPath("(UIElement.RenderTransform).(TranslateTransform.Y)"));

        // Add the animation to the storyboard
        storyboard.Children.Add(doubleAnimationX1);
        storyboard.Children.Add(doubleAnimationX2);
        storyboard.Children.Add(doubleAnimationY1);
        storyboard.Children.Add(doubleAnimationY2);

        // Start the storyboard
        storyboard.Completed += (s, e) =>
        {
            /** 
             RenderTransform needs a reset because the position is not changing the position so it gets a "fake" position 
            and then we need to set the position relative to the canvas
            */
            if (isShipHit) {
                rocket.Source = new BitmapImage(new Uri("pack://application:,,,/505-GUI-Battleships;component/Ressources/RingRed.png", UriKind.RelativeOrAbsolute));
            } 
            else
            {
                rocket.Source = new BitmapImage(new Uri("pack://application:,,,/505-GUI-Battleships;component/Ressources/RingBlue.png", UriKind.RelativeOrAbsolute));
            }
            rocket.RenderTransform = new TranslateTransform();
            Canvas.SetLeft(rocket, clickPosition.X);
            Canvas.SetTop(rocket, clickPosition.Y);
            IsInputEnabled = true;

            
        };
        IsInputEnabled = false;
        storyboard.Begin();
    }

    /*  TODO:
     * private void AnimateElement(ref Storyboard storyboard, UIElement target, double fromX, double toX, double fromY, double toY, TimeSpan duration)
    {

        // Create a DoubleAnimation for the X property
        var doubleAnimationX = new DoubleAnimation();
        doubleAnimationX.From = fromX;
        doubleAnimationX.To = toX;
        doubleAnimationX.BeginTime = new TimeSpan(beginTime)
        doubleAnimationX.Duration = new Duration(duration);

        // Create a DoubleAnimation for the Y property
        var doubleAnimationY = new DoubleAnimation();
        doubleAnimationY.From = fromY;
        doubleAnimationY.To = toY;
        doubleAnimationY.BeginTime = new TimeSpan(beginTime)
        doubleAnimationY.Duration = new Duration(duration);

        // Set the target and target properties for the animations
        Storyboard.SetTarget(doubleAnimationX, target);
        Storyboard.SetTargetProperty(doubleAnimationX, new PropertyPath("(UIElement.RenderTransform).(TranslateTransform.X)"));
        Storyboard.SetTarget(doubleAnimationY, target);
        Storyboard.SetTargetProperty(doubleAnimationY, new PropertyPath("(UIElement.RenderTransform).(TranslateTransform.Y)"));

        // Add the animations to the storyboard
        storyboard.Children.Add(doubleAnimationX);
        storyboard.Children.Add(doubleAnimationY);

        // Start the storyboard
    }*/

}

using _505_GUI_Battleships.Core;
using _505_GUI_Battleships.MVVM.Model;
using _505_GUI_Battleships.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace _505_GUI_Battleships.MVVM.ViewModel;

internal sealed class BoardAttackViewModel: ObservableObject
{
    private PlayerModel _targetedPlayerCard;
    private PlayerModel _attackerPlayerCard;
    private Canvas _playerBoard;
    private Grid _boardContainer;
    private readonly (int Width, int Height) _boardDimensions;
    private readonly GameDataService _gameService;

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

    public void SetupBoardAttack(PlayerModel targetedPlayerCard, PlayerModel attackerPlayerCard/*, Canvas playerBoard*/)
    {
        TargetedPlayerCard = targetedPlayerCard;
        AttackerPlayerCard = attackerPlayerCard;
        OnPropertyChanged(nameof(AttackerPlayerCard));
        /*PlayerBoard = playerBoard;*/
    }
    public BoardAttackViewModel()
    {
        _gameService = GameDataService.GetInstance();
        _boardDimensions = (_gameService.GameBoard!.Width, _gameService.GameBoard!.Height);
        SetupBoardContainer();
        TestAufrufe();
    }

    private void TestAufrufe()
    {
        SetupBoardAttack(_gameService.PlayerModels[1], _gameService.PlayerModels[0]);
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

        _playerBoard = _gameService.GameBoard!.Board;
        _playerBoard.MouseLeftButtonDown += MouseLeftButtonDownHandler;
        _playerBoard.ClipToBounds = false;
        Grid.SetRow(_playerBoard, 1);
        Grid.SetColumn(_playerBoard, 1);

        BoardContainer.Children.Add(_playerBoard);
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
        TestAnimation(clickPosition);
    }

    private void TestAnimation(Point clickPosition)
    {

        BitmapImage imageSource = new BitmapImage(new Uri("pack://application:,,,/505-GUI-Battleships;component/Ressources/Rocket.png", UriKind.RelativeOrAbsolute));

        Image rect1 = new Image()
        {
            Width = 1,
            Height = 1, 
            Source = imageSource
        };

        TranslateTransform tt = new TranslateTransform();
        rect1.RenderTransform = tt;
        //RotateTransform rotateTransform = new RotateTransform();
        //rect1.RenderTransform = rotateTransform;
        //rotateTransform.Angle = 180;
        int index = _playerBoard.Children.Add(rect1);
        Image rect = (Image)_playerBoard.Children[index];
        Storyboard storyboard = new Storyboard();

        double g = 90.8;
        double h = 0;
        double k = _boardDimensions.Height/2 - rect.Height;
        double v = 50;
        double angle = 45;
        double rad = angle * Math.PI / 180;
        double t = (2 * v * Math.Sin(rad)) / g;
        double x = (v * Math.Cos(rad)) * t;

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
        Storyboard.SetTarget(doubleAnimationX1, rect1);
        Storyboard.SetTargetProperty(doubleAnimationX1, new PropertyPath("(UIElement.RenderTransform).(TranslateTransform.X)"));
        Storyboard.SetTarget(doubleAnimationY1, rect1);
        Storyboard.SetTargetProperty(doubleAnimationY1, new PropertyPath("(UIElement.RenderTransform).(TranslateTransform.Y)"));

        Storyboard.SetTarget(doubleAnimationX2, rect1);
        Storyboard.SetTargetProperty(doubleAnimationX2, new PropertyPath("(UIElement.RenderTransform).(TranslateTransform.X)"));
        Storyboard.SetTarget(doubleAnimationY2, rect1);
        Storyboard.SetTargetProperty(doubleAnimationY2, new PropertyPath("(UIElement.RenderTransform).(TranslateTransform.Y)"));

        // Add the animation to the storyboard
        storyboard.Children.Add(doubleAnimationX1);
        storyboard.Children.Add(doubleAnimationX2);
        storyboard.Children.Add(doubleAnimationY1);
        storyboard.Children.Add(doubleAnimationY2);

        // Start the storyboard
        storyboard.Begin();

    }
}

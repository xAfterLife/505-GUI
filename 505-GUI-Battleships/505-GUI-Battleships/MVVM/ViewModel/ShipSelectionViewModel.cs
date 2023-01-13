using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using _505_GUI_Battleships.Core;
using _505_GUI_Battleships.MVVM.Model;
using _505_GUI_Battleships.Services;

namespace _505_GUI_Battleships.MVVM.ViewModel;

internal class ShipSelectionViewModel : ObservableObject, IDisposable
{
    private readonly (int Width, int Height) _boardDimensions;
    private readonly GameDataService _gameService;
    private readonly int _shipAmount;

    private Grid _boardContainer;
    private PlayerModel _currentPlayer;
    private int _currentPlayerCounter;

    private Visibility _nextPlayerBttonVisibility;
    private string _shipPlacementHeading;

    public Canvas PlacementShips { get; set; }

    public Grid BoardContainer
    {
        get => _boardContainer;
        set => Update(ref _boardContainer, value);
    }

    public Visibility NextPlayerButtonVisible
    {
        get => _nextPlayerBttonVisibility;
        set => Update(ref _nextPlayerBttonVisibility, value);
    }

    public string ShipPlacementHeading
    {
        get => _shipPlacementHeading;
        set => Update(ref _shipPlacementHeading, value);
    }

    public ICommand NextPlayerCommand => new RelayCommand(_ =>
    {
        if ( _currentPlayerCounter == _gameService.PlayerModels.Count - 1 )
        {
            if ( _gameService.PlayerModels.Count == 2 )
            {
                _gameService.CurrentTarget = _gameService.PlayerModels.First(x => x != _currentPlayer);
                ChangeViewModel.ChangeView(ChangeViewModel.ViewType.BoardAttack, this);
            }
            else
                ChangeViewModel.ChangeView(ChangeViewModel.ViewType.SelectTargetPlayer, this);
            return;
        }

        _currentPlayerCounter++;
        _currentPlayer = _gameService.PlayerModels[_currentPlayerCounter];
        ShipPlacementHeading = $"Place your ships, {_currentPlayer.PlayerName}!";
        NextPlayerButtonVisible = Visibility.Hidden;

        InstantiateShips();
        GetPlayerBoard().Children.Clear();
    });

    public ShipSelectionViewModel()
    {
        _gameService = GameDataService.GetInstance();
        _boardDimensions = (_gameService.GameBoard!.Width, _gameService.GameBoard!.Height);
        _shipAmount = _gameService.ShipModels.Count;
        _currentPlayerCounter = 0;
        _currentPlayer = _gameService.PlayerModels[_currentPlayerCounter];

        ShipPlacementHeading = $"Place your ships, {_currentPlayer.PlayerName}!";
        NextPlayerButtonVisible = Visibility.Hidden;

        SetupBoardContainer();

        PlacementShips = new Canvas
        {
            AllowDrop = true,
            Height = _shipAmount * 2,
            Width = 6,
            LayoutTransform = new ScaleTransform(1, -1),
            Background = new DrawingBrush
            {
                TileMode = TileMode.Tile,
                Viewbox = new Rect(0, 0, 1, 1),
                Viewport = new Rect(0, 0, 1, 1),
                ViewboxUnits = BrushMappingMode.Absolute,
                ViewportUnits = BrushMappingMode.Absolute
            }
        };

        InstantiateShips();
    }

    public void Dispose() {}

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

        var playerBoard = _gameService.GameBoard!.Board;
        Grid.SetRow(playerBoard, 1);
        Grid.SetColumn(playerBoard, 1);

        playerBoard.DragOver += (_, e) => DragBoardDrop(e, playerBoard, _boardDimensions);
        playerBoard.DragEnter += (sender, e) => BoardEnter(e, playerBoard);

        BoardContainer.Children.Add(playerBoard);
    }

    private Canvas GetPlayerBoard()
    {
        return BoardContainer.Children.OfType<Canvas>().FirstOrDefault(x => x.Uid == "PlayerBoard")!;
    }

    public void DragBoardDrop(DragEventArgs e, Canvas playerBoard, (int Width, int Height) boardDimensions)
    {
        var data = e.Data.GetData(DataFormats.Serializable);

        if ( data is not Image element || !playerBoard.Children.Contains(element) )
            return;

        var dropPosition = e.GetPosition(playerBoard);
        dropPosition.X = Math.Round(dropPosition.X - 0.5);
        dropPosition.Y = Math.Round(dropPosition.Y - 0.5);

        if ( dropPosition.X + element.Width > boardDimensions.Width )
            dropPosition.X = boardDimensions.Width - element.Width;

        if ( dropPosition.Y + element.Height > boardDimensions.Height )
            dropPosition.Y = boardDimensions.Height - element.Height;

        var collision = DetectCollision(playerBoard, element, dropPosition);
        if ( collision )
            return;

        Canvas.SetTop(element, dropPosition.Y);
        Canvas.SetLeft(element, dropPosition.X);

        _currentPlayer.Ships[playerBoard.Children.IndexOf(element)] = new ShipPlacementModel((int)dropPosition.X, (int)dropPosition.Y, true, _gameService.ShipModels[playerBoard.Children.IndexOf(element)].Id, _gameService.ShipModels[playerBoard.Children.IndexOf(element)].Length);

        Trace.WriteLine(dropPosition);
    }

    private static bool DetectCollision(Panel playerBoard, FrameworkElement element, Point dropPosition)
    {
        var placementBounds = new Rect(dropPosition, new Size(element.Width - 1, element.Height - 1));

        foreach ( var ship in playerBoard.Children.OfType<Image>() )
        {
            if ( element == ship )
                continue;
            var otherRect = new Rect(Canvas.GetLeft(ship), Canvas.GetTop(ship), ship.Width - 1, ship.Height - 1);

            if ( placementBounds.IntersectsWith(otherRect) )
                return true;
        }

        return false;
    }

    private void BoardEnter(DragEventArgs e, Panel playerBoard)
    {
        var data = e.Data.GetData(DataFormats.Serializable);

        if ( data is not Image element || !PlacementShips.Children.Contains(element) )
            return;
        var shipListsYPosition = -0.5 * Canvas.GetTop(element) - 1 + _shipAmount;
        PlacementShips.Children.Remove(element);
        playerBoard.Children.Add(element);
        Canvas.SetLeft(element, 0);
        Canvas.SetTop(element, 0);

        if ( !FixElementPositionIfCollision(playerBoard, element) )
        {
            playerBoard.Children.Remove(element);
            PlacementShips.Children.Add(element);
            Canvas.SetTop(element, GetShipListsYPosition(shipListsYPosition));
        }

        CheckIfAllShipsArePlaced();
    }

    private bool FixElementPositionIfCollision(Panel playerBoard, FrameworkElement element)
    {
        var dropPosition = new Point(Canvas.GetLeft(element), Canvas.GetTop(element));
        var initialPos = dropPosition;
        var collision = DetectCollision(playerBoard, element, dropPosition);

        while ( collision )
        {
            dropPosition.X += 1;

            if ( dropPosition.X + element.Width - 1 >= _boardDimensions.Width )
            {
                dropPosition.X = 0;
                if ( dropPosition.Y + element.Height >= _boardDimensions.Height )
                    dropPosition.Y = 0;
                else
                    dropPosition.Y += 1;
            }

            if ( dropPosition == initialPos )
                return false;

            collision = DetectCollision(playerBoard, element, dropPosition);
        }

        Canvas.SetLeft(element, dropPosition.X);
        Canvas.SetTop(element, dropPosition.Y);
        return true;
    }

    private void CheckIfAllShipsArePlaced()
    {
        if ( PlacementShips.Children.Count != 0 )
            return;
        NextPlayerButtonVisible = Visibility.Visible;
    }

    private double GetShipListsYPosition(double shipPosition)
    {
        return _shipAmount * 2 - 2 - shipPosition * 2;
    }

    private void InstantiateShips()
    {
        for ( var i = 0; i < _gameService.ShipModels.Count; i++ )
        {
            var shipModelData = _gameService.ShipModels[i];

            var ship = new Image
            {
                Source = shipModelData.PathHorizontal,
                Height = 1,
                Width = shipModelData.Length,
                UseLayoutRounding = false,
                Stretch = Stretch.Fill,
                RenderTransformOrigin = new Point(0.5, 0.5)
            };

            ship.MouseMove += (_, e) =>
            {
                if ( e.LeftButton == MouseButtonState.Pressed )
                    DragDrop.DoDragDrop(ship, new DataObject(DataFormats.Serializable, ship), DragDropEffects.Move);
            };

            ship.MouseRightButtonDown += (sender, e) =>
            {
                var playerBoard = GetPlayerBoard();
                if ( e.RightButton != MouseButtonState.Pressed || !playerBoard.Children.Contains(ship) )
                    return;
                var initialPosition = new Point(Canvas.GetLeft(ship), Canvas.GetTop(ship));
                PerformFlip(e, ship, shipModelData, playerBoard);

                if ( FixElementPositionIfCollision(playerBoard, ship) )
                    return;

                Canvas.SetLeft(ship, initialPosition.X);
                Canvas.SetTop(ship, initialPosition.Y);
                PerformFlip(e, ship, shipModelData, playerBoard);
            };

            PlacementShips.Children.Add(ship);
            Canvas.SetTop(PlacementShips.Children[i], GetShipListsYPosition(i));
        }
    }

    private void PerformFlip(MouseEventArgs e, Image ship, ShipModel shipModelData, IInputElement playerBoard)
    {
        var shipPlacementModel = _currentPlayer.Ships[_gameService.ShipModels.IndexOf(shipModelData)];
        var length = shipModelData.Length;

        if ( !shipPlacementModel.Horizontal )
        {
            ship.Width = length;
            ship.Height = 1;
            if ( e.GetPosition(playerBoard).X + length > _boardDimensions.Width )
                Canvas.SetLeft(ship, _boardDimensions.Width - length);
        }
        else
        {
            ship.Width = 1;
            ship.Height = length;
            if ( e.GetPosition(playerBoard).Y + length > _boardDimensions.Height )
                Canvas.SetTop(ship, _boardDimensions.Height - length);
        }

        shipPlacementModel.Flip();
        ship.Source = shipModelData.UpdateImageSource(shipPlacementModel.Horizontal);
    }
}

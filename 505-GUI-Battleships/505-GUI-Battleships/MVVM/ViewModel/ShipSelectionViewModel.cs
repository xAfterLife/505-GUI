﻿using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using _505_GUI_Battleships.Core;
using _505_GUI_Battleships.MVVM.Model;
using _505_GUI_Battleships.Services;

namespace _505_GUI_Battleships.MVVM.ViewModel;

internal class ShipSelectionViewModel : ObservableObject
{
    private readonly int _boardSize;
    private readonly GameDataService _gameService;
    private readonly int _shipAmount;

    private int _currentPlayer;

    public ObservableCollection<ShipModel> ShipData { get; set; } = new();
    public ObservableCollection<Image> Ships { get; set; } = new();
    public ObservableCollection<Canvas> PlayerBoards { get; set; } = new();
    public ObservableCollection<Canvas> Shiplists { get; set; } = new();
    public Grid TestGrid { get; set; }
    public Canvas CurrentPlayerBoard { get; set; }
    public Canvas CurrentShiplist { get; set; }
    public bool NextPlayerButtonEnabled { get; set; }
    public string ShipPlacementHeading { get; set; }
    public Visibility NextPlayerButtonVisible { get; set; }
    public ICommand NextPlayerCommand { get; }

    /* CONFIG DATA */

    public ShipSelectionViewModel()
    {
        _gameService = GameDataService.GetInstance();

        _boardSize = _gameService.GameOptions!.BoardWidth;
        _shipAmount = _gameService.GameOptions.ShipLengthList.Count;

        //TODO: Use _gameService
        if ( _gameService.GameBoard != null )
            _boardSize = _gameService.GameBoard.Height;
        else
            //boardSize = 10;

            NextPlayerCommand = new RelayCommand(_ => NextPlayerButtonClick());
        NextPlayerButtonEnabled = false;
        NextPlayerButtonVisible = Visibility.Hidden;

        /* LOAD AMOUNT OF PLAYERS*/
        for ( var i = _currentPlayer; i < _gameService.PlayerModels.Count; i++ )
        {
            InstantiateBoards(i);
            InstantiateShiplists(i);
            InstantiateShips(i);
        }

        SetupBoardGrid();
        CurrentPlayerBoard = PlayerBoards[0];
        Grid.SetRow(CurrentPlayerBoard, 1);
        Grid.SetColumn(CurrentPlayerBoard, 1);

        CreateYAxis();
        CreateXAxis();
        TestGrid?.Children.Add(CurrentPlayerBoard);
        CurrentShiplist = Shiplists[0];
        ShipPlacementHeading = $"Place your ships, {_gameService.PlayerModels[_currentPlayer].PlayerName}!";
    }

    private void SetupBoardGrid()
    {
        TestGrid = new Grid();
        var row1 = new RowDefinition();
        var row2 = new RowDefinition();
        row1.Height = new GridLength(1, GridUnitType.Star);
        row2.Height = new GridLength(_boardSize, GridUnitType.Star);
        var col1 = new ColumnDefinition();
        var col2 = new ColumnDefinition();
        col1.Width = new GridLength(1, GridUnitType.Star);
        col2.Width = new GridLength(_boardSize, GridUnitType.Star);

        TestGrid.RowDefinitions.Add(row1);
        TestGrid.RowDefinitions.Add(row2);
        TestGrid.ColumnDefinitions.Add(col1);
        TestGrid.ColumnDefinitions.Add(col2);
    }

    private void CreateXAxis()
    {
        Canvas xAxis = new() { Width = _boardSize, Height = 1, LayoutTransform = new ScaleTransform(1, -1) };
        Collection<TextBlock> tb = new();

        for ( var i = 0; i < _boardSize; i++ )
        {
            tb.Add(new TextBlock
            {
                Text = ((char)('A' + i)).ToString(),
                FontSize = 0.5,
                Foreground = new SolidColorBrush(Colors.LightGreen),
                LayoutTransform = new ScaleTransform(1, -1),
                TextAlignment = TextAlignment.Center,
                VerticalAlignment = VerticalAlignment.Stretch,
                Width = 1,
                Height = 1
            });
            xAxis.Children.Add(tb[i]);
            //Canvas.SetLeft(tb[i], -0.25);
            Canvas.SetLeft(tb[i], i);
        }

        Grid.SetRow(xAxis, 0);
        Grid.SetColumn(xAxis, 1);
        TestGrid.Children.Add(xAxis);
    }

    private void CreateYAxis()
    {
        Canvas yAxis = new() { Width = 1, Height = _boardSize, LayoutTransform = new ScaleTransform(1, -1) };
        Collection<TextBlock> tb = new();

        for ( var i = 0; i < _boardSize; i++ )
        {
            tb.Add(new TextBlock
            {
                Text = (i + 1).ToString(),
                FontSize = 0.5,
                Foreground = new SolidColorBrush(Colors.White),
                LayoutTransform = new ScaleTransform(1, -1),
                TextAlignment = TextAlignment.Center,
                VerticalAlignment = VerticalAlignment.Stretch,
                Width = 1,
                Height = 1
            });
            yAxis.Children.Add(tb[i]);
            //Canvas.SetLeft(tb[i], -0.25);
            Canvas.SetTop(tb[i], i - 0.2);
        }

        Grid.SetRow(yAxis, 1);
        Grid.SetColumn(yAxis, 0);
        TestGrid.Children.Add(yAxis);
    }

    public void InstantiateShiplists(int currentPlayer)
    {
        Shiplists.Add(new Canvas());
        Shiplists[currentPlayer].AllowDrop = true;
        Shiplists[currentPlayer].Height = _shipAmount * 2;
        Shiplists[currentPlayer].Width = 6;
        Shiplists[currentPlayer].LayoutTransform = new ScaleTransform(1, -1);
        DrawingBrush db = new()
        {
            // https://learn.microsoft.com/en-us/dotnet/api/system.windows.media.tilebrush.viewbox?view=windowsdesktop-7.0
            TileMode = TileMode.Tile,
            Viewbox = new Rect(0, 0, 1, 1),
            Viewport = new Rect(0, 0, 1, 1),
            ViewboxUnits = BrushMappingMode.Absolute,
            ViewportUnits = BrushMappingMode.Absolute
        };
        Shiplists[currentPlayer].Background = db;
    }

    private void InstantiateBoards(int currentPlayer)
    {
        PlayerBoards.Add(new Canvas
        {
            Width = _boardSize,
            Height = _boardSize,
            ClipToBounds = true,
            AllowDrop = true,
            LayoutTransform = new ScaleTransform(1, -1)
        });

        DrawingBrush brush = new()
        {
            TileMode = TileMode.Tile,
            Viewbox = new Rect(0, 0, 1, 1),
            Viewport = new Rect(0, 0, 1, 1),
            ViewboxUnits = BrushMappingMode.Absolute,
            ViewportUnits = BrushMappingMode.Absolute,
            Drawing = new GeometryDrawing { Pen = new Pen { Thickness = 0.1, Brush = Brushes.DarkGreen }, Geometry = new RectangleGeometry(new Rect(0, 0, 1, 1)) }
        };

        PlayerBoards[currentPlayer].Background = brush;
        PlayerBoards[currentPlayer].AllowDrop = true;
        PlayerBoards[currentPlayer].DragOver += (sender, e) => DragBoardDrop(e, PlayerBoards[currentPlayer], _boardSize);
        PlayerBoards[currentPlayer].DragEnter += (sender, e) => BoardEnter(e, PlayerBoards[currentPlayer], Shiplists[currentPlayer]);
        PlayerBoards[currentPlayer].DragOver += (sender, e) => DragBoardDrop(e, PlayerBoards[currentPlayer], _boardSize);
        PlayerBoards[currentPlayer].DragEnter += (sender, e) => BoardEnter(e, PlayerBoards[currentPlayer], Shiplists[currentPlayer]);
    }

    private void InstantiateShips(int currentPlayer)
    {
        //TODO: Config Datei verwenden (?)
        var urisources = new Collection<Uri>
        {
            new("pack://application:,,,/505-GUI-Battleships;component/Ressources/Ships/1ShipPatrolHorizontal.png"),
            new("pack://application:,,,/505-GUI-Battleships;component/Ressources/Ships/1ShipPatrolVertical.png"),
            new("pack://application:,,,/505-GUI-Battleships;component/Ressources/Ships/2ShipRescueHorizontal.png"),
            new("pack://application:,,,/505-GUI-Battleships;component/Ressources/Ships/2ShipRescueVertical.png"),
            new("pack://application:,,,/505-GUI-Battleships;component/Ressources/Ships/3ShipSubMarineHorizontal.png"),
            new("pack://application:,,,/505-GUI-Battleships;component/Ressources/Ships/3ShipSubMarineVertical.png"),
            new("pack://application:,,,/505-GUI-Battleships;component/Ressources/Ships/4ShipDestroyerHorizontal.png"),
            new("pack://application:,,,/505-GUI-Battleships;component/Ressources/Ships/4ShipDestroyerVertical.png"),
            new("pack://application:,,,/505-GUI-Battleships;component/Ressources/Ships/5ShipBattleshipHorizontal.png"),
            new("pack://application:,,,/505-GUI-Battleships;component/Ressources/Ships/5ShipBattleshipVertical.png")
        };
        var bitmapImages = new Collection<BitmapImage>();
        foreach ( var uri in urisources )
            bitmapImages.Add(new BitmapImage(uri));

        for ( var i = 0; i < _shipAmount; i++ )
        {
            switch ( _gameService.GameOptions!.ShipLengthList[i] )
            {
                case 1:
                    ShipData.Add(new ShipModel(1, true, bitmapImages[0], bitmapImages[1]));
                    break;

                case 2:
                    ShipData.Add(new ShipModel(2, true, bitmapImages[2], bitmapImages[3]));
                    break;

                case 3:
                    ShipData.Add(new ShipModel(3, true, bitmapImages[4], bitmapImages[5]));
                    break;

                case 4:
                    ShipData.Add(new ShipModel(4, true, bitmapImages[6], bitmapImages[7]));
                    break;

                case 5:
                    ShipData.Add(new ShipModel(5, true, bitmapImages[8], bitmapImages[9]));
                    break;
            }

            Shiplists[currentPlayer].Children.Add(CreateShip(ShipData[i], currentPlayer));
            Canvas.SetTop(Shiplists[currentPlayer].Children[i], GetShipListsYPosition(i));
        }
    }

    private double GetShipListsYPosition(double shipPosition)
    {
        return _shipAmount * 2 - 2 - shipPosition * 2;
    }

    private void BoardEnter(DragEventArgs e, Canvas playerBoard, Panel shipList)
    {
        var data = e.Data.GetData(DataFormats.Serializable);

        if ( data is not Image element || !shipList.Children.Contains(element) )
            return;
        var shipListsYPosition = -0.5 * Canvas.GetTop(element) - 1 + _shipAmount;
        shipList.Children.Remove(element);
        playerBoard.Children.Add(element);
        Canvas.SetLeft(element, 0);
        Canvas.SetTop(element, 0);

        if ( !FixElementPositionIfCollision(playerBoard, element) )
        {
            playerBoard.Children.Remove(element);
            shipList.Children.Add(element);
            Canvas.SetTop(element, GetShipListsYPosition(shipListsYPosition));
        }

        CheckIfAllShipsArePlaced();
    }

    private void CheckIfAllShipsArePlaced()
    {
        if ( Shiplists[_currentPlayer].Children.Count != 0 )
            return;
        NextPlayerButtonEnabled = true;
        NextPlayerButtonVisible = Visibility.Visible;
        OnPropertyChanged(nameof(NextPlayerButtonEnabled));
        OnPropertyChanged(nameof(NextPlayerButtonVisible));
    }

    public void NextPlayerButtonClick()
    {
        if ( _currentPlayer < _gameService.PlayerModels.Count - 1 )
        {
            BoatPositionTest();
            _currentPlayer++;
            CurrentPlayerBoard = PlayerBoards[_currentPlayer];
            TestGrid.Children.RemoveAt(2);
            Grid.SetRow(CurrentPlayerBoard, 1);
            Grid.SetColumn(CurrentPlayerBoard, 1);
            TestGrid.Children.Add(CurrentPlayerBoard);
            CurrentShiplist = Shiplists[_currentPlayer];
            OnPropertyChanged(nameof(CurrentShiplist));
            OnPropertyChanged(nameof(TestGrid));

            ShipPlacementHeading = $"Place your ships, {_gameService.PlayerModels[_currentPlayer].PlayerName}!";
            OnPropertyChanged(nameof(ShipPlacementHeading));

            NextPlayerButtonEnabled = false;
            NextPlayerButtonVisible = Visibility.Hidden;
            OnPropertyChanged(nameof(NextPlayerButtonEnabled));
            OnPropertyChanged(nameof(NextPlayerButtonVisible));
        }
        else
        {
            BoatPositionTest();
        }
    }

    private void BoatPositionTest()
    {
        for ( var i = 0; i < _gameService.PlayerModels.Count; i++ )
        {
            Trace.WriteLine("Board : " + i);

            foreach ( Image ship in PlayerBoards[i].Children )
            {
                var shipPosition = new Point(Canvas.GetLeft(ship), Canvas.GetTop(ship));
                Trace.WriteLine(shipPosition);
            }
        }
    }

    /** function that creates the UIElement of type Image that represents the ship*/
    private Image CreateShip(ShipModel shipModelData, int i)
    {
        var newShip = new Image { Source = shipModelData.PathHorizontal, Height = 1, Width = shipModelData.Length };
        newShip.MouseMove += (o, e) => ShipMouseMove(e, newShip);
        newShip.MouseRightButtonDown += (sender, e) => RightClickFlip(e, newShip, shipModelData, PlayerBoards[i]);
        newShip.RenderTransformOrigin = new Point(0.5, 0.5);
        newShip.Stretch = Stretch.Fill;
        newShip.UseLayoutRounding = false;
        return newShip;
    }

    public static void ShipMouseMove(MouseEventArgs e, Image ship)
    {
        if ( e.LeftButton == MouseButtonState.Pressed )
            DragDrop.DoDragDrop(ship, new DataObject(DataFormats.Serializable, ship), DragDropEffects.Move);
    }

    private bool FixElementPositionIfCollision(Panel playerBoard, FrameworkElement element)
    {
        var dropPosition = new Point(Canvas.GetLeft(element), Canvas.GetTop(element));
        var initialPos = dropPosition;
        var collision = DetectCollision(playerBoard, element, dropPosition);

        while ( collision )
        {
            dropPosition.X += 1;

            if ( dropPosition.X + element.Width - 1 >= _boardSize )
            {
                dropPosition.X = 0;
                if ( dropPosition.Y + element.Height >= _boardSize )
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

    private static bool DetectCollision(Panel playerBoard, FrameworkElement element, Point dropPosition)
    {
        foreach ( Image otherShip in playerBoard.Children )
        {
            if ( element == otherShip )
                continue;

            /** Prevents collision with other ships*/

            var otherLeft = Canvas.GetLeft(otherShip);
            var otherTop = Canvas.GetTop(otherShip);
            if ( (dropPosition.X >= otherLeft - element.Width + 1) & (dropPosition.X <= otherLeft + otherShip.Width - 1) & (dropPosition.Y >= otherTop - element.Height + 1) & (dropPosition.Y <= otherTop + otherShip.Height - 1) )
                return true;
        }

        return false;
    }

    public static void DragBoardDrop(DragEventArgs e, Canvas playerBoard, int boardSize)
    {
        var data = e.Data.GetData(DataFormats.Serializable);

        if ( data is not Image element || !playerBoard.Children.Contains(element) )
            return;

        //** Snaps the ships on the generated grid*/
        var dropPosition = e.GetPosition(playerBoard);
        dropPosition.X = Math.Round(dropPosition.X - 0.5);
        dropPosition.Y = Math.Round(dropPosition.Y - 0.5);

        //** Prevents ship from leaving the board*/
        if ( dropPosition.X + element.Width > boardSize )
            dropPosition.X = boardSize - element.Width;

        if ( dropPosition.Y + element.Height > boardSize )
            dropPosition.Y = boardSize - element.Height;

        var collision = DetectCollision(playerBoard, element, dropPosition);
        if ( collision )
            return;

        Canvas.SetTop(element, dropPosition.Y);
        Canvas.SetLeft(element, dropPosition.X);
        Trace.WriteLine(dropPosition);
    }

    private void PerformFlip(MouseEventArgs e, Image ship, ShipModel shipModelData, IInputElement playerBoard)
    {
        var length = shipModelData.Length;

        if ( !shipModelData.Horizontal )
        {
            ship.Width = length;
            ship.Height = 1;
            if ( e.GetPosition(playerBoard).X + length > _boardSize )
                Canvas.SetLeft(ship, _boardSize - length);
        }
        else
        {
            ship.Width = 1;
            ship.Height = length;
            if ( e.GetPosition(playerBoard).Y + length > _boardSize )
                Canvas.SetTop(ship, _boardSize - length);
        }

        shipModelData.Flip();
        ship.Source = shipModelData.UpdateImageSource();
    }

    private void RightClickFlip(MouseEventArgs e, Image ship, ShipModel shipModelData, Canvas playerBoard)
    {
        if ( e.RightButton != MouseButtonState.Pressed || !playerBoard.Children.Contains(ship) )
            return;
        var initialPosition = new Point(Canvas.GetLeft(ship), Canvas.GetTop(ship));
        PerformFlip(e, ship, shipModelData, playerBoard);

        if ( FixElementPositionIfCollision(playerBoard, ship) )
            return;

        Canvas.SetLeft(ship, initialPosition.X);
        Canvas.SetTop(ship, initialPosition.Y);
        PerformFlip(e, ship, shipModelData, playerBoard);
    }
}

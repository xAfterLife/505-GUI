using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using _505_GUI_Battleships.Core;
using _505_GUI_Battleships.MVVM.Model;

namespace _505_GUI_Battleships.MVVM.ViewModel;

internal class ShipSelectionViewModel : ObservableObject
{
    private readonly int _boardSize = 13;
    private readonly int _playerAmount = 4;
    private readonly int _totalShiplenght = 15;
    private int _currentPlayer;

    public ObservableCollection<ShipModel> ShipData { get; set; } = new();
    public ObservableCollection<Image> Ships { get; set; } = new();
    public ObservableCollection<Canvas> PlayerBoards { get; set; } = new();
    public ObservableCollection<Canvas> Shiplists { get; set; } = new();
    public Canvas CurrentPlayerBoard { get; set; }
    public Canvas CurrentShiplist { get; set; }
    public bool NextPlayerButtonEnabled { get; set; }
    public Visibility NextPlayerButtonVisible { get; set; }

    public ICommand NextPlayerCommand { get; }

    /* CONFIG DATA */
    public int RequiredLengthValue { get; set; }

    public ShipSelectionViewModel()
    {
        NextPlayerCommand = new RelayCommand(_ => NextPlayerButtonClick());
        NextPlayerButtonEnabled = false;
        NextPlayerButtonVisible = Visibility.Hidden;
        RequiredLengthValue = _totalShiplenght;

        /* LOAD AMOUNT OF PLAYERS*/
        for ( var i = _currentPlayer; i < _playerAmount; i++ )
        {
            RequiredLengthValue = _totalShiplenght;
            InstantiateBoards(i);
            InstantiateShiplists(i);
            InstantiateShips(i);
        }

        CurrentPlayerBoard = PlayerBoards[0];
        CurrentShiplist = Shiplists[0];
    }

    public void InstantiateShiplists(int currentPlayer)
    {
        Shiplists.Add(new Canvas());
        Shiplists[currentPlayer].AllowDrop = true;
        Shiplists[currentPlayer].Height = 10;
        Shiplists[currentPlayer].Width = 6;
        Shiplists[currentPlayer].DragLeave += (sender, e) => ListDragLeave(sender, e, PlayerBoards[currentPlayer], Shiplists[currentPlayer], currentPlayer);
        //Shiplists[i].DragOver += new DragEventHandler((sender, e) => Dragfrom)
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
        /** 
         * Could Move Board Config in XAML 
         * **/
        PlayerBoards.Add(new Canvas());
        PlayerBoards[currentPlayer].Height = _boardSize;
        PlayerBoards[currentPlayer].Width = _boardSize;
        PlayerBoards[currentPlayer].ClipToBounds = true;
        PlayerBoards[currentPlayer].AllowDrop = true;
        PlayerBoards[currentPlayer].DragOver += (sender, e) => DragBoardDrop(sender, e, PlayerBoards[currentPlayer], _boardSize);
        PlayerBoards[currentPlayer].LayoutTransform = new ScaleTransform(1, -1);
        DrawingBrush db = new()
        {
            // https://learn.microsoft.com/en-us/dotnet/api/system.windows.media.tilebrush.viewbox?view=windowsdesktop-7.0
            TileMode = TileMode.Tile,
            Viewbox = new Rect(0, 0, 1, 1),
            Viewport = new Rect(0, 0, 1, 1),
            ViewboxUnits = BrushMappingMode.Absolute,
            ViewportUnits = BrushMappingMode.Absolute
        };
        GeometryDrawing gd = new();
        RectangleGeometry rec = new(new Rect(0, 0, 1, 1));
        Pen gdPen = new() { Thickness = 0.1, Brush = Brushes.DarkGreen };
        db.Drawing = gd;
        gd.Pen = gdPen;
        gd.Geometry = rec;
        PlayerBoards[currentPlayer].Background = db;
    }

    private void InstantiateShips(int currentPlayer)
    {
        /**
         * Generierung der Schiffe mit Bild TODO: Config Datei verwenden (?)
         **/
        BitmapImage logo = new();
        logo.BeginInit();
        logo.UriSource = new Uri("pack://application:,,,/505-GUI-Battleships;component/Resources/ShipRescueHorizontal.png");
        logo.EndInit();
        ShipData.Add(new ShipModel(1, true, 1, 1, logo));
        ShipData.Add(new ShipModel(2, true, 1, 1, logo));
        ShipData.Add(new ShipModel(3, true, 1, 1, logo));
        ShipData.Add(new ShipModel(4, true, 1, 1, logo));
        ShipData.Add(new ShipModel(5, true, 1, 1, logo));

        RefreshList(currentPlayer);
    }

    public void NextPlayerButtonClick()
    {
        if ( _currentPlayer <= _playerAmount - 1 )
        {
            BoatPositionTest();
            RequiredLengthValue = _totalShiplenght;
            _currentPlayer++;
            CurrentPlayerBoard = PlayerBoards[_currentPlayer];
            CurrentShiplist = Shiplists[_currentPlayer];
            OnPropertyChanged(nameof(CurrentPlayerBoard));
            OnPropertyChanged(nameof(CurrentShiplist));
            OnPropertyChanged(nameof(RequiredLengthValue));

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
        for ( var i = 0; i < _playerAmount; i++ )
        {
            Trace.WriteLine("Board :");

            foreach ( Image ship in PlayerBoards[i].Children )
            {
                var shipPosition = new Point(Canvas.GetLeft(ship), Canvas.GetTop(ship));
                Trace.WriteLine(shipPosition);
            }
        }
    }

    private void RefreshList(int currentPlayer)
    {
        Ships.Clear();
        var lenghtcap = 5;
        if ( RequiredLengthValue < 5 )
            lenghtcap = RequiredLengthValue;

        if ( RequiredLengthValue == 0 )
        {
            //ENABLE NEXT PLAYER BUTTON
            NextPlayerButtonEnabled = true;
            NextPlayerButtonVisible = Visibility.Visible;
            OnPropertyChanged(nameof(NextPlayerButtonEnabled));
            OnPropertyChanged(nameof(NextPlayerButtonVisible));
        }

        for ( var j = 0; j < lenghtcap; j++ )
        {
            Ships.Add(CreateShip(ShipData[j], currentPlayer));
            Shiplists[currentPlayer].Children.Add(Ships[j]);
            Canvas.SetTop(Shiplists[currentPlayer].Children[j], 8 - j * 2);
        }
    }

    /** function that creates the UIElement of type Image that represents the ship*/
    private Image CreateShip(ShipModel shipModelData, int i)
    {
        var newShip = new Image { Source = shipModelData.Path, Height = 1, Width = shipModelData.Length };
        newShip.MouseMove += (sender, e) => ShipMouseMove(sender, e, newShip);
        newShip.MouseRightButtonDown += (sender, e) => RightClickFlip(sender, e, newShip, shipModelData, PlayerBoards[i]);
        newShip.RenderTransformOrigin = new Point(0.5, 0.5);
        newShip.Stretch = Stretch.Fill;
        newShip.UseLayoutRounding = false;
        return newShip;
    }

    public void ShipMouseMove(object sender, MouseEventArgs e, Image ship)
    {
        //object data = e.Data

        if ( e.LeftButton == MouseButtonState.Pressed )
            DragDrop.DoDragDrop(ship, new DataObject(DataFormats.Serializable, ship), DragDropEffects.Move);
    }

    private void ListDragLeave(object sender, DragEventArgs e, Canvas playerBoard, Canvas shipList, int currentPlayer)
    {
        var data = e.Data.GetData(DataFormats.Serializable);
        //Trace.WriteLine(data, "DragLeave");

        /** Adds the selected ship to the board*/
        if ( data is not Image element || !shipList.Children.Contains(element) )
            return;

        shipList.Children.Clear();
        playerBoard.Children.Add(element);
        FixElementPositionIfCollision(playerBoard, element);
        var currentShipLength = (int)element.Width + (int)element.Height - 1;
        RequiredLengthValue -= currentShipLength;
        OnPropertyChanged(nameof(RequiredLengthValue));
        //Trace.WriteLine(requiredLengthValue);
        RefreshList(currentPlayer);
    }

    private void FixElementPositionIfCollision(Canvas playerBoard, Image element)
    {
        Canvas.SetLeft(element, 0);
        var dropPosition = new Point(Canvas.GetLeft(element), Canvas.GetTop(element));
        var collision = DetectCollision(playerBoard, element, dropPosition);

        while ( collision )
        {
            var xPos = Canvas.GetLeft(element);
            var yPos = Canvas.GetTop(element);
            Canvas.SetLeft(element, xPos + 1);

            if ( xPos + element.Width > _boardSize )
            {
                //if (yPos)
                Canvas.SetLeft(element, 0);
                Canvas.SetTop(element, yPos + 1);
            }

            dropPosition = new Point(xPos, Canvas.GetTop(element));
            collision = DetectCollision(playerBoard, element, dropPosition);
            Trace.WriteLine("Collision");
        }
    }

    private bool DetectCollision(Canvas playerBoard, Image element, Point dropPosition)
    {
        foreach ( Image otherShip in playerBoard.Children )
        {
            if ( element == otherShip )
                continue;

            /** Prevents collision with other ships*/

            var otherLeft = Canvas.GetLeft(otherShip);
            var otherTop = Canvas.GetTop(otherShip);
            if (
                (dropPosition.X >= otherLeft - element.Width + 1) & (dropPosition.X <= otherLeft + otherShip.Width - 1) &
                (dropPosition.Y >= otherTop - element.Height + 1) & (dropPosition.Y <= otherTop + otherShip.Height - 1)
            )
                return true;
        }

        return false;
    }

    public void DragBoardDrop(object sender, DragEventArgs e, Canvas playerBoard, int boardSize)
    {
        var data = e.Data.GetData(DataFormats.Serializable);

        if ( data is not Image element || !playerBoard.Children.Contains(element) )
            return;

        /** Snaps the ships on the generated grid*/
        var dropPosition = e.GetPosition(playerBoard);
        dropPosition.X = Math.Round(dropPosition.X - 0.5);
        dropPosition.Y = Math.Round(dropPosition.Y - 0.5);

        /** Prevents ship from leaving the board*/
        if ( dropPosition.X + element.Width > boardSize )
            dropPosition.X = boardSize - element.Width;

        if ( dropPosition.Y + element.Height > boardSize )
            dropPosition.Y = boardSize - element.Height;

        var collision = DetectCollision(playerBoard, element, dropPosition);

        if ( collision )
            return;

        Canvas.SetTop(element, dropPosition.Y);
        Canvas.SetLeft(element, dropPosition.X);
        //Trace.WriteLine(dropPosition);
    }

    //TODO: Implement logic to prevent shipstacking
    private void RightClickFlip(object sender, MouseButtonEventArgs e, Image ship, ShipModel shipModelData, Canvas playerBoard)
    {
        if ( e.RightButton != MouseButtonState.Pressed || !playerBoard.Children.Contains(ship) )
            return;

        Trace.WriteLine("Rightclick Pressed");
        var length = shipModelData.Length;

        if ( !shipModelData.Horizontal )
        {
            shipModelData.Flip();
            ship.Width = length;
            ship.Height = 1;
            var xPos = e.GetPosition(playerBoard).X;
            if ( xPos + length > _boardSize )
                Canvas.SetLeft(ship, _boardSize - length);
        }
        else
        {
            shipModelData.Flip();
            ship.Width = 1;
            ship.Height = length;
            var yPos = e.GetPosition(playerBoard).Y;
            if ( yPos + length > _boardSize )
                Canvas.SetTop(ship, _boardSize - length);
        }
    }
}

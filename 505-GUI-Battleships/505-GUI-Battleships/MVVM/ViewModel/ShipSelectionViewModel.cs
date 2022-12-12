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
    private const int BoardWith = 10;
    private const int BoardHeight = 10;

    private readonly int _currentPlayer = 0;

    private readonly SelectBoard _selectBoard = new();
    private int _playerAmount;
    public ObservableCollection<Ship> ShipData { get; set; } = new();
    public ObservableCollection<Image> Ships { get; set; } = new();
    public ObservableCollection<Canvas> PlayerBoards { get; set; } = new();
    public Canvas PlayerBoard { get; set; }
    public ObservableCollection<PlayerSelectionModel> Players { get; set; }

    public ShipSelectionViewModel()
    {
        /* LOAD AMOUNT OF PLAYERS*/
        _playerAmount = 1;
        InstantiateBoards(_currentPlayer);
        InstantiateShips(_currentPlayer);
        PlayerBoard = PlayerBoards[_currentPlayer];
    }

    private void InstantiateBoards(int i)
    {
        /** 
         * Data from external Config
         * **/
        PlayerBoards.Add(new Canvas());
        PlayerBoards[i].Height = BoardHeight;
        PlayerBoards[i].Width = BoardWith;
        PlayerBoards[i].ClipToBounds = true;
        PlayerBoards[i].AllowDrop = true;
        PlayerBoards[i].DragOver += (sender, e) => _selectBoard.DragBoardDrop(sender, e, PlayerBoards[i]);
        PlayerBoards[i].LayoutTransform = new ScaleTransform(1, -1);

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
        PlayerBoards[i].Background = db;
    }

    private void InstantiateShips(int i)
    {
        /**
         * Generierung von PseudoSchiff mit Bild TODO:´Ships von externer Config Datei einlesen TODO++: Configdatei modifizierbar machen in StartView.Einstellungen
         **/
        BitmapImage logo = new();
        logo.BeginInit();
        logo.UriSource = new Uri("pack://application:,,,/505-GUI-Battleships;component/Resources/ShipRescue.png");
        logo.EndInit();
        ShipData.Add(new Ship(3, false, 1, 1, logo));

        Ships.Add(new Image());
        Ships[i].Source = ShipData[0].Path;
        Ships[i].Height = ShipData[0].Length;
        Ships[i].Width = 1;
        Ships[i].MouseMove += (sender, e) => _selectBoard.ShipMouseMove(sender, e, Ships[i]);
        Ships[i].MouseRightButtonDown += (sender, e) => RightClickFlip(e, Ships[i], ShipData[i], PlayerBoards[i]);
        Ships[i].RenderTransformOrigin = new Point(0.5, 0.5);
        Ships[i].Stretch = Stretch.Fill;
        Ships[i].UseLayoutRounding = false;
        PlayerBoards[i].Children.Add(Ships[i]);
    }

    private static void RightClickFlip(MouseEventArgs e, FrameworkElement ship, Ship shipData, IInputElement playerBoard)
    {
        if ( e.RightButton != MouseButtonState.Pressed )
            return;

        Trace.WriteLine("Rightclick Pressed");
        var length = shipData.Length;
        var pos = e.GetPosition(playerBoard);

        ship.Width = shipData.Horizontal ? 1 : length;
        ship.Height = shipData.Horizontal ? length : 1;

        switch ( shipData.Horizontal )
        {
            case false when pos.X + length > 10:
                Canvas.SetLeft(ship, 10 - length);
                break;
            case true when pos.Y + length > 10:
                Canvas.SetTop(ship, 10 - length);
                break;
        }

        shipData.Flip();
    }
}

using _505_GUI_Battleships.Core;
using _505_GUI_Battleships.MVVM.Model;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace _505_GUI_Battleships.MVVM.ViewModel
{
    internal class ShipSelectionViewModel : ObservableObject
    {
        public ObservableCollection<Ship> ShipData { get; set; } = new ObservableCollection<Ship>();
        public ObservableCollection<Image> Ships { get; set; } = new ObservableCollection<Image>();
        public ObservableCollection<Canvas> PlayerBoards { get; set; } = new ObservableCollection<Canvas>();
        public Canvas PlayerBoard { get; set; }

        public ObservableCollection<PlayerSelectionModel> Players { get; set; }

        private SelectBoard selectBoard = new();
        private int currentPlayer = 0;
        private int playerAmount;

        public ShipSelectionViewModel()
        {
            /* LOAD AMOUNT OF PLAYERS*/ 
            int playerAmount = 1;
                InstantiateBoards(currentPlayer);
                InstantiateShips(currentPlayer);
                PlayerBoard = PlayerBoards[currentPlayer];
        }

        private void InstantiateBoards(int i)
        {
            /** 
             * Data from external Config
             * **/
            int cWidth = 10;
            int cHeight = 10;
            PlayerBoards.Add(new Canvas());
            PlayerBoards[i].Height = cHeight;
            PlayerBoards[i].Width = cWidth;
            PlayerBoards[i].ClipToBounds = true;
            PlayerBoards[i].AllowDrop = true;
            PlayerBoards[i].DragOver += new DragEventHandler((sender, e) => selectBoard.DragBoardDrop(sender, e, PlayerBoards[i]));
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
            Pen gdPen = new()
            {
                Thickness = 0.1,
                Brush = Brushes.DarkGreen
            };
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
            Ships[i].Source = ShipData[0].GetPath();
            Ships[i].Height = ShipData[0].GetLength();
            Ships[i].Width = 1;
            Ships[i].MouseMove += new MouseEventHandler((sender, e) => selectBoard.ShipMouseMove(sender, e, Ships[i]));
            Ships[i].MouseRightButtonDown += new MouseButtonEventHandler((sender, e) => RightClickFlip(sender, e, Ships[i], ShipData[i], PlayerBoards[i]));
            Ships[i].RenderTransformOrigin = new Point(0.5, 0.5);
            Ships[i].Stretch = Stretch.Fill;
            Ships[i].UseLayoutRounding = false;
            PlayerBoards[i].Children.Add(Ships[i]);
        }

        private void RightClickFlip(object sender, MouseButtonEventArgs e, Image ship, Ship shipData, Canvas playerBoard)
        {
            if (e.RightButton == MouseButtonState.Pressed)
            {
                Trace.WriteLine("Rightclick Pressed");
                int length = shipData.GetLength();
                if (!shipData.IsHorizontal())
                {
                    shipData.Flip();
                    ship.Width = length;
                    ship.Height = 1;
                    double xPos = e.GetPosition(playerBoard).X;
                    if (xPos + length > 10)
                    {
                        Canvas.SetLeft(ship, 10 - length);
                    }
                }
                else
                {
                    shipData.Flip();
                    ship.Width = 1;
                    ship.Height = length;
                    double yPos = e.GetPosition(playerBoard).Y;
                    if (yPos + length > 10)
                    {
                        Canvas.SetTop(ship, 10 - length);
                    }
                }
            }
        }
    }
}
